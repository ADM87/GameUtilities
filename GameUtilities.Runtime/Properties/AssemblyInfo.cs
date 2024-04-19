using System.Runtime.CompilerServices;

// Expose internals to the unit test assembly.
[assembly: InternalsVisibleTo("Tests.Fail")]
[assembly: InternalsVisibleTo("Tests.Pass")]
