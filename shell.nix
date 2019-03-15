{ pkgs ? import <nixpkgs> {} }:

with pkgs;
  mkShell {
    buildInputs = [
      mono
      dotnet-sdk
      dotnetPackages.Nuget
    ];
}
