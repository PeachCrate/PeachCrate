DROP TRIGGER IF EXISTS tr_AddUser
GO
CREATE TRIGGER tr_AddUser
ON Users
FOR INSERT
AS
BEGIN

-- Отримуємо ідентифікатор нового користувача
DECLARE @UserId INT;
SET @UserId = (SELECT UserId
            FROM inserted);

-- Створюємо новий запис у таблиці Groups
INSERT INTO Groups
(Title, Description, InvintationCode, CreationDate, IsUserGroup)
VALUES
((SELECT Login FROM inserted), NULL, NULL, (SELECT RegistrationDate FROM inserted), 1);

-- Отримуємо ідентифікатор нової групи
DECLARE @GroupId INT;
SET @GroupId = (SELECT GroupId
            FROM Groups
            WHERE Title = (SELECT Login
                         FROM inserted));

-- Створюємо новий запис у таблиці GroupUser
INSERT INTO GroupUser
(GroupsGroupId, UsersUserId)
VALUES
(@GroupId, @UserId);

END;
go



DROP TRIGGER IF EXISTS tr_UpdateUser
GO
CREATE TRIGGER tr_UpdateUser
    ON Users
    FOR UPDATE
    AS
BEGIN

    -- Отримуємо логін оновлюваного користувача
    DECLARE @Login NVARCHAR(50);
    SET @Login = (SELECT Login FROM inserted);

    DECLARE @UserId INT;
    SET @UserId = (SELECT UserId FROM inserted);

    DECLARE @LoginBefore NVARCHAR(50);
    SET @LoginBefore = (SELECT Login
                        FROM Users
                        WHERE UserId = @UserId)

    IF (@Login != @LoginBefore)
        BEGIN
            DECLARE @GroupId INT;
            SET @GroupId = (SELECT GroupId
                            FROM Groups
                            WHERE Title = @Login);

-- Оновлюємо назву групи з логіном оновлюваного користувача
            UPDATE Groups
            SET Title = @Login
            WHERE GroupId = @GroupId;
        END
END;
go



DROP TRIGGER IF EXISTS tr_DeleteUser
GO
CREATE TRIGGER tr_DeleteUser
ON Users
FOR DELETE
AS
BEGIN

-- Отримуємо логін користувача, що видаляємо
DECLARE @Login NVARCHAR(50);
DECLARE @UserId INT;
SET @Login  = (SELECT Login FROM deleted);
SET @UserId = (SELECT UserId FROM deleted)

-- Отримуємо ідентифікатор групи з логіном користувача, що видаляємо
DECLARE @GroupId INT;
SET @GroupId = (SELECT GroupId
            FROM Groups
            WHERE Title = @Login);


-- Видаляємо групу
DELETE FROM Groups
WHERE GroupId = @GroupId;

-- Видаляємо запис із таблиці GroupUser
DELETE FROM GroupUser
WHERE GroupsGroupId = @GroupId AND UsersUserId = @UserId;

END;
go

