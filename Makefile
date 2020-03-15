run:
	dotnet run -p ConsoleTest

class:
	dotnet run -p ConsoleTest -- ToTestTest:445

namespace:
	dotnet run -p ConsoleTest -- %OtherNamespace%

namespace-one:
	dotnet run -p ConsoleTest -- %space.one%:445

line:
	dotnet run -p ConsoleTest -- ToTestInjection:74

not-filter:
	dotnet run -p ConsoleTest -- NotExistedClass

not-option:
	dotnet run -p ConsoleTest -- -non-existed-option

summary:
	dotnet run -p ConsoleTest -- -v summary

verbose:
	dotnet run -p ConsoleTest -- -v verbose

verbose-line:
	dotnet run -p ConsoleTest -- -v verbose -l

verbose-time:
	dotnet run -p ConsoleTest -- -v verbose -t

verbose-all:
	dotnet run -p ConsoleTest -- -v verbose -tl

async:
	dotnet run -p ConsoleTest -- -a

async-summary:
	dotnet run -p ConsoleTest -- -v summary -a