run:
	dotnet run -p BddSpec.ConsoleTest

summary:
	dotnet run -p BddSpec.ConsoleTest -- -v summary

verbose:
	dotnet run -p BddSpec.ConsoleTest -- -v verbose

verbose-line:
	dotnet run -p BddSpec.ConsoleTest -- -v verbose -l

verbose-time:
	dotnet run -p BddSpec.ConsoleTest -- -v verbose -t

verbose-all:
	dotnet run -p BddSpec.ConsoleTest -- -v verbose -tl

async:
	dotnet run -p BddSpec.ConsoleTest -- -a

async-summary:
	dotnet run -p BddSpec.ConsoleTest -- -v summary -a