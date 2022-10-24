#!/usr/bin/env bash

set -o errexit

(

script_dir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
cd "$script_dir"

function error_exit {
  echo $1
  echo ""
  echo "Usage: init.sh YEAR LANG"
  exit 1
}

if [[ $# -ne 2 ]]; then
  error_exit "Wrong number of arguments"
fi

YEAR=$1
LANG=$2

template_root=".templates/$LANG"
if [ ! -d "$template_root" ]; then
  error_exit "Unknown LANG: $LANG"
fi

data_root="$YEAR/_inputs"
output_root="$YEAR/$LANG"
if [ -d "$output_root" ]; then
  error_exit "Existing directory: $output_root"
fi
mkdir -p "$output_root"

name="Aoc$YEAR"
sln_path="$output_root/$name.sln"
dotnet new sln -o "$output_root" -n "$name"

dotnet new console -lang 'F#' -o "$output_root/$name" -n "$name"
cp "$template_root/Aoc.fsproj" "$output_root/$name/$name.fsproj"
cp "$template_root/Program.fs" "$output_root/$name/"
cp "$template_root/Utils.fs" "$output_root/$name/"
dotnet sln "$output_root/$name.sln" add "$output_root/$name"

dotnet new xunit -lang 'F#' -o "$output_root/$name.Tests" -n "$name.Tests"
cp "$template_root/Aoc.Tests.fsproj" "$output_root/$name.Tests/$name.Tests.fsproj"
rm "$output_root/$name.Tests/Tests.fs"
dotnet add "$output_root/$name.Tests/$name.Tests.fsproj" reference "$output_root/$name"
dotnet sln "$output_root/$name.sln" add "$output_root/$name.Tests"

cp "$template_root/Program.fs" "$output_root/$name/"
mkdir -p "$data_root"
mkdir -p "$data_root"
for i in {01..25}; do
  touch "$data_root/Day$i.txt"
  sed -e "s/XY/$i/g" <"$template_root/Day.fs" >"$output_root/$name/Day$i.fs"
  sed -e "s/XY/$i/g" <"$template_root/Day.Tests.fs" >"$output_root/$name.Tests/Day$i.Tests.fs"
done

git add "$output_root" "$data_root"
git commit -m "Init $output_root"

)
