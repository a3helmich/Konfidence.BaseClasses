

### Konfidence.BaseClasses
- Some extensions to make reading some patterns more fluent
	- when objects/string/guid/datatime/timespan assignments are actually assigned, .IsAssigned()
	- eof for stream reading, .IsEof()
	- string is a guid, .IsGuid()
	- string is numeric,  .IsNumeric()
	- earliest and latest time on a day, .StartOfDayTime(), .EndOfDayTime()
- CommandLineArgument parser: Meant for the configuration argument line parser used with MS dependency injection.
- Environment Variable getter: unified(user, machine, process). Should work on both Windows and Linux.
- default configured Json serializer/deserializer, based on System.Text.Json.
- Some (unexpected) fast string extensions
	- TrimStart(..), TrimStartIgnoreCase(..), TrimEnd(..), TrimEndIgnoreCase(..)
	- TrimList()
	- ReplaceIgnoreCase(..)
	- InitLowerCase(), InitUpperCase()
	- Contains(..) with specified casing type — superseded by the framework's own `string.Contains(string, StringComparison)`, which wins overload resolution against an extension method of the same signature
	- ToDecimal(), parse a string into a decimal
- Wpf/BaseViewModel: an `INotifyPropertyChanged` base class with a `SetField(..)` change-detecting setter and a nesting `SuppressNotifications()` scope
 
- currently targets net9.0 and net10.0

**Breaking change in 2026.4**: `Wpf/BaseViewModel.SetFrozenField(..)` was removed.
### The Konfidence.BaseClasses repo is available on [github.com](https://github.com/a3helmich/Konfidence.BaseClasses).

