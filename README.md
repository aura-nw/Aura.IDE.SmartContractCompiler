<h1 align="center">Welcome to Aura IDE</h1>

<p>
  <img alt="Version" src="https://img.shields.io/badge/.net-6-blue" />
  <img alt="Version" src="https://img.shields.io/badge/cargo-v1.55.0%2B-yellowgreen" />
</p>

# Installation
## Rust
The standard approach is to use rustup to maintain dependencies and handle updating multiple versions of cargo and rustc, which you will be using.

After install [rustup tool](https://rustup.rs/) make sure you have the wasm32 target:
```
rustup target list --installed
rustup target add wasm32-unknown-unknown
```

## .NET6 SDK

Installing with APT can be done with a few commands. Before you install .NET, run the following commands to add the Microsoft package signing key to your list of trusted keys and add the package repository.

Open a terminal and run the following commands:
```
wget https://packages.microsoft.com/config/ubuntu/21.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
```

Install the SDK
```
sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-6.0
```

## Running the app
```
dotnet run
```

# Deploying the app
Edit the config file **appsettings.Production.json** and run the following commands
```
set ASPNETCORE_ENVIRONMENT=Production
dotnet publish Aura.IDE.RustCompiler.csproj -c Release -r ubuntu-x64 --self-contained false
cd bin/Release/net6.0/ubuntu-x64/publish
dotnet Aura.IDE.SmartContractCompiler.dll --urls http://0.0.0.0:5002
```

# License
[MIT](https://github.com/aura-nw/flower-store-contract/blob/main/LICENSE) License.
