using System.Collections;
using Models.Accounts;

namespace Models.Banks;

public record Bank(
    long id,
    string name,
    long adminEntryKey);