# PAY.ON .NET

The unofficial PAY.ON .NET library built using .NET Standard.

# Getting Started

## Prerequisites

.NET Core 2.2 is required to build this solution.

## Build

Build the solution by running:

```
dotnet restore
dotnet build
```

## Test

Run units using the following command:

```
dotnet test
```

## Release

Create a published release by running:

```
dotnet publish src/PayOn/PayOn.csproj -o build/
```

# Usage

Please see the [PayOnClientTests.cs](https://github.com/andrehaupt/payon-dotnet/blob/main/tests/PayOn.Tests/PayOnClientTests.cs) test class for a variety of usage cases.

# Source Code

The source code is stored in GitHub. Access to the repository would requires a user account and the Git client to be installed locally.

It is recommended to use SSH with GitHub as it offers increased security and is simpler to use daily. Setting up SSH is beyond the scope of this document, but in short, one has to generate a public and private key, which will be stored under your `/home/username/.ssh` and `C:\Users\UserName\.ssh` directory for MacOS and Windows respectively. The _key_.pub file contents should be added under your [GitHub SSH keys section](https://github.com/settings/keys).

After your keys have been configured, you should be able to clone the source code. The repository can be cloned using the following two commands:

```
git clone git@github.com:andrehaupt/payon-dotnet.git
```

## Project Structure

The project is structured as follows([1](https://github.com/kriasoft/Folder-Structure-Conventions)):

    .
    ├── build                   # Compiled files
    ├── docs                    # Documentation files
    ├── src                     # Source files
    ├── tests                   # Automated tests
    ├── tools                   # Tools and utilities
    ├── LICENSE
    ├── CHANGELOG.md
    └── README.md

> Use short lowercase names for top-level files and folders except:
> `LICENSE`, `README.md`

## Versioning

[Semantic Versioning 2.0.0](http://semver.org/) is used for this project.

## Changelog

### [1.0.0](https://github.com/andrehaupt/payon-dotnet/releases/tag/1.0.0) - 2020-11-22

First public release.

## License

MIT License.

## Authors

- **André Hauptfleisch** - [GitHub Profile](https://github.com/andrehaupt)
