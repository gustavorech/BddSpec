run:
	dotnet run -p BddSpec.ConsoleTest

class:
	dotnet run -p BddSpec.ConsoleTest -- ToTestTest

namespace:
	dotnet run -p BddSpec.ConsoleTest -- %OtherNamespace%

namespace-one:
	dotnet run -p BddSpec.ConsoleTest -- %space.one%

not:
	dotnet run -p BddSpec.ConsoleTest -- NotExistedClass

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