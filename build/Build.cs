using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
	[Parameter] string NugetApiUrl = "https://api.nuget.org/v3/index.json";
	[Parameter] string NugetApiKey;

	[Solution] readonly Solution Solution;
	[GitRepository] readonly GitRepository GitRepository;
	[GitVersion] readonly GitVersion GitVersion;

	[Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
	readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

	AbsolutePath SourceDirectory => RootDirectory / "src";

	AbsolutePath TestsDirectory => RootDirectory / "tests";

	AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

	AbsolutePath PackageDirectory => ArtifactsDirectory / "nuget";

	Target Clean => _ => _
		.Before(Restore)
		.Executes(() =>
		{
			SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
			TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
			EnsureCleanDirectory(ArtifactsDirectory);
		});

	Target Restore => _ => _
		.Executes(() =>
		{
			DotNetRestore(s => s
				.SetProjectFile(Solution));
		});

	Target Compile => _ => _
		.DependsOn(Restore)
		.Executes(() =>
		{
			DotNetBuild(s => s
				.SetProjectFile(Solution)
				.SetConfiguration(Configuration)
				.SetAssemblyVersion(GitVersion.AssemblySemVer)
				.SetFileVersion(GitVersion.AssemblySemFileVer)
				.SetInformationalVersion(GitVersion.InformationalVersion)
				.EnableNoRestore());
		});

	Target Pack => _ => _
		.DependsOn(Compile)
		.Executes(() =>
		{
			DotNetPack(s => s
				.SetProject(Solution.GetProject("Octogram.Contracts"))
				.SetConfiguration(Configuration)
				.EnableNoBuild()
				.EnableNoRestore()
				.SetDescription("Global Octogram contracts")
				.SetPackageTags("Octogram", "Contracts")
				.SetAuthors("sankovlex@gmail.com")
				.SetPackageProjectUrl("https://github.com/Octogram/Octogram.Contracts")
				.SetPackageLicenseUrl("https://github.com/Octogram/Octogram.Contracts/blob/master/LICENSE")
				.SetVersion(GitVersion.NuGetVersionV2)
				.SetNoDependencies(true)
				.SetOutputDirectory(PackageDirectory));
		});

	Target Push => _ => _
		.DependsOn(Pack)
		.Requires(() => NugetApiUrl)
		.Requires(() => NugetApiKey)
		.Executes(() =>
		{
			GlobFiles(PackageDirectory, "*.nupkg")
				.NotEmpty()
				.Where(file => !file.EndsWith("symbols.nupkg"))
				.ForEach(package =>
				{
					DotNetNuGetPush(s => s
						.SetTargetPath(package)
						.SetSource(NugetApiUrl)
						.SetApiKey(NugetApiKey));
				});
		});

	/// Support plugins are available for:
	/// - JetBrains ReSharper        https://nuke.build/resharper
	/// - JetBrains Rider            https://nuke.build/rider
	/// - Microsoft VisualStudio     https://nuke.build/visualstudio
	/// - Microsoft VSCode           https://nuke.build/vscode
	public static int Main() => Execute<Build>(x => x.Compile);
}
