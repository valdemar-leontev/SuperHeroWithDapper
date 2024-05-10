CREATE   PROCEDURE GetSuperHeroByName
@Name NVARCHAR(100)
AS
BEGIN
    SELECT Id, Name, FirstName, LastName, Place
    FROM SuperHero
    WHERE Name = @Name;
END
go

