namespace Models.Enums;

public enum UserOrderBy
{
    ByUserIdASC, ByLoginASC, ByEmailASC, ByRegistrationDateASC,
    ByUserIdDESC, ByLoginDESC, ByEmailDESC, ByRegistrationDateDESC
}

public enum UserFilterBy
{
    NoFilter, ByLogin, ByEmail, ByRegistrationDateBefore, ByRegistrationDateAfter
}