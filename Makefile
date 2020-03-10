run:
	dotnet run -p BddSpec.ConsoleTest

class:
	dotnet run -p BddSpec.ConsoleTest -- ToTestTest

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