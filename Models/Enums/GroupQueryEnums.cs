namespace Models.Enums;

public enum GroupOrderBy
{
    ByGroupIdASC, ByTitleASC, ByDescriptionASC, ByCreationDateASC, ByGroupIdDESC, ByTitleDESC, ByDescriptionDESC, ByCreationDateDESC
}

public enum GroupFilterBy
{
    NoFilter, ByTitle, ByDescription, ByCreationDate
}