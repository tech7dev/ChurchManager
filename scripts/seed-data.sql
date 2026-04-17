-- ============================================================================
-- ChurchMS — Comprehensive Seed Data
-- UTF-8 encoding with French/African accented names
-- ============================================================================
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET QUOTED_IDENTIFIER ON;

BEGIN TRANSACTION;

-- ============================================================================
-- 1. CHURCHES (Parent + Children)
-- ============================================================================
DECLARE @ParentChurchId UNIQUEIDENTIFIER = 'A0000001-0000-0000-0000-000000000001';
DECLARE @ChildChurch1Id UNIQUEIDENTIFIER = 'A0000001-0000-0000-0000-000000000002';
DECLARE @ChildChurch2Id UNIQUEIDENTIFIER = 'A0000001-0000-0000-0000-000000000003';
DECLARE @Now DATETIME = GETUTCDATE();

INSERT INTO Churches (Id, Name, Code, [Description], [Address], City, [State], Country, Phone, Email, Website, TimeZone, PrimaryCurrency, SecondaryCurrency, [Status], SubscriptionPlan, ParentChurchId, CreatedAt, IsDeleted)
VALUES
(@ParentChurchId, N'Église Évangélique de la Grâce', 'EEG-001', N'Assemblée principale — siège central de l''Église Évangélique de la Grâce', N'45 Boulevard de la Liberté', N'Douala', N'Littoral', N'Cameroun', '+237 699 123 456', 'contact@eeg-grace.org', 'https://eeg-grace.org', 'Africa/Douala', 'XAF', 'EUR', 1, 4, NULL, @Now, 0),
(@ChildChurch1Id, N'Église de la Grâce — Yaoundé Centre', 'EEG-YDE', N'Antenne de Yaoundé Centre', N'12 Rue Joseph Mballa Eloumden', N'Yaoundé', N'Centre', N'Cameroun', '+237 677 234 567', 'yaounde@eeg-grace.org', NULL, 'Africa/Douala', 'XAF', NULL, 1, 3, @ParentChurchId, @Now, 0),
(@ChildChurch2Id, N'Église de la Grâce — Bafoussam', 'EEG-BFS', N'Antenne de Bafoussam', N'8 Avenue des Chefs', N'Bafoussam', N'Ouest', N'Cameroun', '+237 655 345 678', 'bafoussam@eeg-grace.org', NULL, 'Africa/Douala', 'XAF', NULL, 1, 2, @ParentChurchId, @Now, 0);

-- ============================================================================
-- 2. USERS (one per role, all for parent church)
-- ============================================================================
-- Role IDs from AspNetRoles
DECLARE @RoleSuperAdmin UNIQUEIDENTIFIER   = '613A4DF9-4BFC-2D32-E134-AB222D849E50';
DECLARE @RoleCentralAdmin UNIQUEIDENTIFIER = 'D03E059B-FEA6-EC2D-E316-6A9526406A17';
DECLARE @RoleChurchAdmin UNIQUEIDENTIFIER  = '181FB183-29CF-98AB-9BF5-0E6586AFBB98';
DECLARE @RoleITManager UNIQUEIDENTIFIER    = '2ED18CE4-0485-3781-7E5A-9FA66C22C168';
DECLARE @RoleSecretary UNIQUEIDENTIFIER    = '6A999513-6810-6DD2-CB84-86DF1E1DB89B';
DECLARE @RoleTreasurer UNIQUEIDENTIFIER    = '73A7F390-B4FE-F40A-0179-9EF804DE8EC0';
DECLARE @RoleDeptHead UNIQUEIDENTIFIER     = '12C86DFE-37D2-A015-0200-3B37E5DDD6CF';
DECLARE @RoleDeptTreas UNIQUEIDENTIFIER    = '1449CAC4-DBD5-ECA9-306F-AE0631614598';
DECLARE @RoleTeacher UNIQUEIDENTIFIER      = '2EE5EE5E-E5C1-9880-9BE9-44EE7CAE917D';
DECLARE @RoleEvangLead UNIQUEIDENTIFIER    = '3565871D-A91B-1B69-044A-2A32F9AAC7B5';
DECLARE @RoleMultimedia UNIQUEIDENTIFIER   = 'F9893397-D7A8-E694-78D9-1C5C9E60D271';
DECLARE @RoleLogistics UNIQUEIDENTIFIER    = '6BF621D7-EA3B-C90E-EB5A-029AAC85EF24';
DECLARE @RoleMember UNIQUEIDENTIFIER       = 'B78F967C-501F-35E3-442B-35062A35620A';

-- User GUIDs
DECLARE @UserCentralAdmin UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000001';
DECLARE @UserChurchAdmin UNIQUEIDENTIFIER  = 'B0000001-0000-0000-0000-000000000002';
DECLARE @UserITManager UNIQUEIDENTIFIER    = 'B0000001-0000-0000-0000-000000000003';
DECLARE @UserSecretary UNIQUEIDENTIFIER    = 'B0000001-0000-0000-0000-000000000004';
DECLARE @UserTreasurer UNIQUEIDENTIFIER    = 'B0000001-0000-0000-0000-000000000005';
DECLARE @UserDeptHead UNIQUEIDENTIFIER     = 'B0000001-0000-0000-0000-000000000006';
DECLARE @UserDeptTreas UNIQUEIDENTIFIER    = 'B0000001-0000-0000-0000-000000000007';
DECLARE @UserTeacher UNIQUEIDENTIFIER      = 'B0000001-0000-0000-0000-000000000008';
DECLARE @UserEvangLead UNIQUEIDENTIFIER    = 'B0000001-0000-0000-0000-000000000009';
DECLARE @UserMultimedia UNIQUEIDENTIFIER   = 'B0000001-0000-0000-0000-00000000000A';
DECLARE @UserLogistics UNIQUEIDENTIFIER    = 'B0000001-0000-0000-0000-00000000000B';
DECLARE @UserMember1 UNIQUEIDENTIFIER      = 'B0000001-0000-0000-0000-00000000000C';
DECLARE @UserMember2 UNIQUEIDENTIFIER      = 'B0000001-0000-0000-0000-00000000000D';
DECLARE @UserMember3 UNIQUEIDENTIFIER      = 'B0000001-0000-0000-0000-00000000000E';

-- All passwords: "Password@123" hashed by ASP.NET Identity v3 (PBKDF2-SHA256)
-- We use a common hash for demo data
DECLARE @PwdHash NVARCHAR(MAX) = N'AQAAAAIAAYagAAAAEGqJUdD8R+fY9VMNu7z7PxImisnRHP4VMy7xQ1JkNfr2XbvMnO5VPx+L3VZ8Nh5oVQ==';
DECLARE @SecurityStamp NVARCHAR(MAX) = N'SEEDSTAMP00000000000000000000000';
DECLARE @ConcurrencyStamp NVARCHAR(MAX) = NEWID();

INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, FirstName, LastName, ChurchId, IsActive, CreatedAt, IsDeleted)
VALUES
(@UserCentralAdmin, 'central@eeg-grace.org',    'CENTRAL@EEG-GRACE.ORG',    'central@eeg-grace.org',    'CENTRAL@EEG-GRACE.ORG',    1, @PwdHash, @SecurityStamp, NEWID(), '+237 699 100 001', 0, 0, 1, 0, N'Éric',      N'Nguéma',     @ParentChurchId, 1, @Now, 0),
(@UserChurchAdmin,  'admin.church@eeg-grace.org','ADMIN.CHURCH@EEG-GRACE.ORG','admin.church@eeg-grace.org','ADMIN.CHURCH@EEG-GRACE.ORG',1, @PwdHash, @SecurityStamp, NEWID(), '+237 699 100 002', 0, 0, 1, 0, N'Hélène',    N'Tchouameni', @ParentChurchId, 1, @Now, 0),
(@UserITManager,    'it@eeg-grace.org',          'IT@EEG-GRACE.ORG',          'it@eeg-grace.org',          'IT@EEG-GRACE.ORG',          1, @PwdHash, @SecurityStamp, NEWID(), '+237 699 100 003', 0, 0, 1, 0, N'Célestin',  N'Mbappé',     @ParentChurchId, 1, @Now, 0),
(@UserSecretary,    'secretariat@eeg-grace.org', 'SECRETARIAT@EEG-GRACE.ORG', 'secretariat@eeg-grace.org', 'SECRETARIAT@EEG-GRACE.ORG', 1, @PwdHash, @SecurityStamp, NEWID(), '+237 699 100 004', 0, 0, 1, 0, N'Béatrice',  N'Fotso',      @ParentChurchId, 1, @Now, 0),
(@UserTreasurer,    'tresorier@eeg-grace.org',   'TRESORIER@EEG-GRACE.ORG',   'tresorier@eeg-grace.org',   'TRESORIER@EEG-GRACE.ORG',   1, @PwdHash, @SecurityStamp, NEWID(), '+237 699 100 005', 0, 0, 1, 0, N'André',     N'Kamga',      @ParentChurchId, 1, @Now, 0),
(@UserDeptHead,     'dept.head@eeg-grace.org',   'DEPT.HEAD@EEG-GRACE.ORG',   'dept.head@eeg-grace.org',   'DEPT.HEAD@EEG-GRACE.ORG',   1, @PwdHash, @SecurityStamp, NEWID(), '+237 699 100 006', 0, 0, 1, 0, N'François',  N'Nganou',     @ParentChurchId, 1, @Now, 0),
(@UserDeptTreas,    'dept.tres@eeg-grace.org',   'DEPT.TRES@EEG-GRACE.ORG',   'dept.tres@eeg-grace.org',   'DEPT.TRES@EEG-GRACE.ORG',   1, @PwdHash, @SecurityStamp, NEWID(), '+237 699 100 007', 0, 0, 1, 0, N'Géraldine', N'Tagne',      @ParentChurchId, 1, @Now, 0),
(@UserTeacher,      'enseignant@eeg-grace.org',  'ENSEIGNANT@EEG-GRACE.ORG',  'enseignant@eeg-grace.org',  'ENSEIGNANT@EEG-GRACE.ORG',  1, @PwdHash, @SecurityStamp, NEWID(), '+237 699 100 008', 0, 0, 1, 0, N'Théophile', N'Nkoulou',    @ParentChurchId, 1, @Now, 0),
(@UserEvangLead,    'evangelisme@eeg-grace.org', 'EVANGELISME@EEG-GRACE.ORG', 'evangelisme@eeg-grace.org', 'EVANGELISME@EEG-GRACE.ORG', 1, @PwdHash, @SecurityStamp, NEWID(), '+237 699 100 009', 0, 0, 1, 0, N'Moïse',     N'Essomba',    @ParentChurchId, 1, @Now, 0),
(@UserMultimedia,   'multimedia@eeg-grace.org',  'MULTIMEDIA@EEG-GRACE.ORG',  'multimedia@eeg-grace.org',  'MULTIMEDIA@EEG-GRACE.ORG',  1, @PwdHash, @SecurityStamp, NEWID(), '+237 699 100 010', 0, 0, 1, 0, N'Noël',      N'Atangana',   @ParentChurchId, 1, @Now, 0),
(@UserLogistics,    'logistique@eeg-grace.org',  'LOGISTIQUE@EEG-GRACE.ORG',  'logistique@eeg-grace.org',  'LOGISTIQUE@EEG-GRACE.ORG',  1, @PwdHash, @SecurityStamp, NEWID(), '+237 699 100 011', 0, 0, 1, 0, N'Raphaël',   N'Djomgang',   @ParentChurchId, 1, @Now, 0),
(@UserMember1,      'membre1@eeg-grace.org',     'MEMBRE1@EEG-GRACE.ORG',     'membre1@eeg-grace.org',     'MEMBRE1@EEG-GRACE.ORG',     1, @PwdHash, @SecurityStamp, NEWID(), '+237 699 100 012', 0, 0, 1, 0, N'Joséphine', N'Mbouda',     @ParentChurchId, 1, @Now, 0),
(@UserMember2,      'membre2@eeg-grace.org',     'MEMBRE2@EEG-GRACE.ORG',     'membre2@eeg-grace.org',     'MEMBRE2@EEG-GRACE.ORG',     1, @PwdHash, @SecurityStamp, NEWID(), '+237 699 100 013', 0, 0, 1, 0, N'Émmanuel',  N'Tchinda',    @ParentChurchId, 1, @Now, 0),
(@UserMember3,      'membre3@eeg-grace.org',     'MEMBRE3@EEG-GRACE.ORG',     'membre3@eeg-grace.org',     'MEMBRE3@EEG-GRACE.ORG',     1, @PwdHash, @SecurityStamp, NEWID(), '+237 699 100 014', 0, 0, 1, 0, N'Élisabeth', N'Nkwenti',    @ChildChurch1Id, 1, @Now, 0);

-- Assign roles
INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES
(@UserCentralAdmin, @RoleCentralAdmin),
(@UserChurchAdmin,  @RoleChurchAdmin),
(@UserITManager,    @RoleITManager),
(@UserSecretary,    @RoleSecretary),
(@UserTreasurer,    @RoleTreasurer),
(@UserDeptHead,     @RoleDeptHead),
(@UserDeptTreas,    @RoleDeptTreas),
(@UserTeacher,      @RoleTeacher),
(@UserEvangLead,    @RoleEvangLead),
(@UserMultimedia,   @RoleMultimedia),
(@UserLogistics,    @RoleLogistics),
(@UserMember1,      @RoleMember),
(@UserMember2,      @RoleMember),
(@UserMember3,      @RoleMember);

-- ============================================================================
-- 3. FAMILIES
-- ============================================================================
DECLARE @Family1 UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000001';
DECLARE @Family2 UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000002';
DECLARE @Family3 UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000003';
DECLARE @Family4 UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000004';

INSERT INTO Families (Id, ChurchId, Name, Notes, CreatedAt, IsDeleted) VALUES
(@Family1, @ParentChurchId,  N'Famille Nguéma',     N'Famille fondatrice',           @Now, 0),
(@Family2, @ParentChurchId,  N'Famille Kamga',      N'Famille du trésorier',         @Now, 0),
(@Family3, @ParentChurchId,  N'Famille Essomba',    N'Famille active en évangélisation', @Now, 0),
(@Family4, @ChildChurch1Id,  N'Famille Nkwenti',    N'Famille de l''antenne Yaoundé', @Now, 0);

-- ============================================================================
-- 4. MEMBERS (15 members with accented names)
-- ============================================================================
DECLARE @Mbr01 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000001';
DECLARE @Mbr02 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000002';
DECLARE @Mbr03 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000003';
DECLARE @Mbr04 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000004';
DECLARE @Mbr05 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000005';
DECLARE @Mbr06 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000006';
DECLARE @Mbr07 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000007';
DECLARE @Mbr08 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000008';
DECLARE @Mbr09 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000009';
DECLARE @Mbr10 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-00000000000A';
DECLARE @Mbr11 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-00000000000B';
DECLARE @Mbr12 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-00000000000C';
DECLARE @Mbr13 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-00000000000D';
DECLARE @Mbr14 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-00000000000E';
DECLARE @Mbr15 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-00000000000F';

INSERT INTO Members (Id, ChurchId, FirstName, MiddleName, LastName, DateOfBirth, Gender, MaritalStatus, Phone, Email, [Address], City, Country, MembershipNumber, [Status], JoinDate, BaptismDate, Occupation, QrCodeValue, FamilyId, FamilyRole, AppUserId, CreatedAt, IsDeleted)
VALUES
(@Mbr01, @ParentChurchId, N'Éric',      N'Jean',     N'Nguéma',     '1978-03-15', 1, 2, '+237 699 100 001', 'central@eeg-grace.org',    N'45 Bd de la Liberté',     N'Douala',    N'Cameroun', 'EEG-0001', 1, '2010-01-15', '2010-06-20', N'Pasteur principal',    'QR-EEG-0001', @Family1, 1, @UserCentralAdmin, @Now, 0),
(@Mbr02, @ParentChurchId, N'Hélène',    N'Marie',    N'Tchouameni', '1982-07-22', 2, 2, '+237 699 100 002', 'admin.church@eeg-grace.org',N'22 Rue Foch',             N'Douala',    N'Cameroun', 'EEG-0002', 1, '2011-03-01', '2011-09-15', N'Administratrice',      'QR-EEG-0002', NULL,     NULL, @UserChurchAdmin, @Now, 0),
(@Mbr03, @ParentChurchId, N'Célestin',  N'Paul',     N'Mbappé',     '1990-11-05', 1, 1, '+237 699 100 003', 'it@eeg-grace.org',          N'10 Rue de la Joie',       N'Douala',    N'Cameroun', 'EEG-0003', 1, '2015-06-10', '2016-01-08', N'Ingénieur informatique','QR-EEG-0003', NULL,     NULL, @UserITManager,   @Now, 0),
(@Mbr04, @ParentChurchId, N'Béatrice',  N'Aimée',    N'Fotso',      '1985-04-18', 2, 2, '+237 699 100 004', 'secretariat@eeg-grace.org', N'5 Rue des Manguiers',     N'Douala',    N'Cameroun', 'EEG-0004', 1, '2012-09-01', '2013-03-10', N'Secrétaire juridique', 'QR-EEG-0004', NULL,     NULL, @UserSecretary,   @Now, 0),
(@Mbr05, @ParentChurchId, N'André',     N'Simon',    N'Kamga',      '1975-01-30', 1, 2, '+237 699 100 005', 'tresorier@eeg-grace.org',   N'18 Avenue Ahmadou Ahidjo',N'Douala',    N'Cameroun', 'EEG-0005', 1, '2009-05-20', '2010-01-12', N'Comptable agréé',      'QR-EEG-0005', @Family2, 1, @UserTreasurer,   @Now, 0),
(@Mbr06, @ParentChurchId, N'François',  N'Étienne',  N'Nganou',     '1980-08-12', 1, 2, '+237 699 100 006', 'dept.head@eeg-grace.org',   N'30 Rue Tokoto',           N'Douala',    N'Cameroun', 'EEG-0006', 1, '2011-01-15', '2011-07-20', N'Directeur commercial', 'QR-EEG-0006', NULL,     NULL, @UserDeptHead,    @Now, 0),
(@Mbr07, @ParentChurchId, N'Géraldine', N'Flore',    N'Tagne',      '1988-12-03', 2, 1, '+237 699 100 007', 'dept.tres@eeg-grace.org',   N'7 Rue Japoma',            N'Douala',    N'Cameroun', 'EEG-0007', 1, '2014-04-01', '2015-01-18', N'Analyste financière',  'QR-EEG-0007', NULL,     NULL, @UserDeptTreas,   @Now, 0),
(@Mbr08, @ParentChurchId, N'Théophile', N'René',     N'Nkoulou',    '1972-06-25', 1, 2, '+237 699 100 008', 'enseignant@eeg-grace.org',  N'14 Rue Bépanda',          N'Douala',    N'Cameroun', 'EEG-0008', 1, '2008-02-10', '2008-08-15', N'Professeur de lycée',  'QR-EEG-0008', NULL,     NULL, @UserTeacher,     @Now, 0),
(@Mbr09, @ParentChurchId, N'Moïse',     N'Samuel',   N'Essomba',    '1983-09-08', 1, 2, '+237 699 100 009', 'evangelisme@eeg-grace.org', N'3 Rue Ndogpassi',         N'Douala',    N'Cameroun', 'EEG-0009', 1, '2010-11-01', '2011-04-22', N'Évangéliste',          'QR-EEG-0009', @Family3, 1, @UserEvangLead,   @Now, 0),
(@Mbr10, @ParentChurchId, N'Noël',      N'Gervais',  N'Atangana',   '1992-12-25', 1, 1, '+237 699 100 010', 'multimedia@eeg-grace.org',  N'20 Rue Deïdo',            N'Douala',    N'Cameroun', 'EEG-0010', 1, '2017-01-08', '2017-06-11', N'Vidéaste',             'QR-EEG-0010', NULL,     NULL, @UserMultimedia,  @Now, 0),
(@Mbr11, @ParentChurchId, N'Raphaël',   N'Dieudonné',N'Djomgang',   '1987-05-14', 1, 2, '+237 699 100 011', 'logistique@eeg-grace.org',  N'9 Rue Bonabéri',          N'Douala',    N'Cameroun', 'EEG-0011', 1, '2013-07-01', '2014-02-16', N'Logisticien',          'QR-EEG-0011', NULL,     NULL, @UserLogistics,   @Now, 0),
(@Mbr12, @ParentChurchId, N'Joséphine', N'Grâce',    N'Mbouda',     '1995-02-28', 2, 1, '+237 699 100 012', 'membre1@eeg-grace.org',     N'16 Rue Akwa',             N'Douala',    N'Cameroun', 'EEG-0012', 1, '2019-03-10', '2019-09-22', N'Infirmière',           'QR-EEG-0012', NULL,     NULL, @UserMember1,     @Now, 0),
(@Mbr13, @ParentChurchId, N'Émmanuel',  N'Thierry',  N'Tchinda',    '1998-10-17', 1, 1, '+237 699 100 013', 'membre2@eeg-grace.org',     N'25 Rue Bonanjo',          N'Douala',    N'Cameroun', 'EEG-0013', 1, '2020-06-01', '2021-01-10', N'Étudiant en médecine', 'QR-EEG-0013', NULL,     NULL, @UserMember2,     @Now, 0),
(@Mbr14, @ChildChurch1Id, N'Élisabeth', N'Colette',  N'Nkwenti',    '1991-08-09', 2, 2, '+237 699 100 014', 'membre3@eeg-grace.org',     N'4 Rue Melen',             N'Yaoundé',   N'Cameroun', 'EEG-0014', 1, '2018-01-15', '2018-07-08', N'Enseignante',          'QR-EEG-0014', @Family4, 1, @UserMember3,     @Now, 0),
(@Mbr15, @ParentChurchId, N'Thérèse',   N'Yvonne',   N'Nguéma',     '1980-04-05', 2, 2, '+237 699 200 015', 'therese.nguema@email.cm',   N'45 Bd de la Liberté',     N'Douala',    N'Cameroun', 'EEG-0015', 1, '2010-01-15', '2010-06-20', N'Femme au foyer',       'QR-EEG-0015', @Family1, 2, NULL,             @Now, 0);

-- ============================================================================
-- 5. VISITORS
-- ============================================================================
INSERT INTO Visitors (Id, ChurchId, FirstName, LastName, Phone, Email, Gender, FirstVisitDate, HowHeardAboutUs, Notes, [Status], CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, N'Stéphane',  N'Kouam',     '+237 670 111 222', 'stephane.k@email.cm',  1, '2026-03-15', N'Invité par un ami',          N'Très intéressé par l''école du dimanche', 1, @Now, 0),
(NEWID(), @ParentChurchId, N'Félicité',  N'Ngongang',  '+237 670 333 444', 'felicite.n@email.cm',  2, '2026-03-22', N'Publicité sur les réseaux',  N'Première visite, a demandé des informations', 2, @Now, 0),
(NEWID(), @ParentChurchId, N'Jérémie',   N'Takam',     '+237 670 555 666', NULL,                   1, '2026-04-01', N'Passait devant l''église',   N'Recherche une communauté chrétienne', 1, @Now, 0),
(NEWID(), @ChildChurch1Id, N'Pélagie',   N'Mfopou',    '+237 670 777 888', 'pelagie.m@email.cm',   2, '2026-04-05', N'Recommandée par un voisin',  N'Vient de déménager à Yaoundé', 3, @Now, 0);

-- ============================================================================
-- 6. FUNDS
-- ============================================================================
DECLARE @FundGeneral UNIQUEIDENTIFIER = 'E0000001-0000-0000-0000-000000000001';
DECLARE @FundBuilding UNIQUEIDENTIFIER = 'E0000001-0000-0000-0000-000000000002';
DECLARE @FundMissions UNIQUEIDENTIFIER = 'E0000001-0000-0000-0000-000000000003';

INSERT INTO Funds (Id, ChurchId, Name, [Description], IsDefault, IsActive, CreatedAt, IsDeleted) VALUES
(@FundGeneral,  @ParentChurchId, N'Fonds général',        N'Dîmes et offrandes courantes',               1, 1, @Now, 0),
(@FundBuilding, @ParentChurchId, N'Fonds de construction', N'Construction du nouveau temple',             0, 1, @Now, 0),
(@FundMissions, @ParentChurchId, N'Fonds des missions',    N'Soutien aux missions d''évangélisation',     0, 1, @Now, 0);

-- ============================================================================
-- 7. CONTRIBUTION CAMPAIGNS
-- ============================================================================
DECLARE @Campaign1 UNIQUEIDENTIFIER = 'E0000002-0000-0000-0000-000000000001';

INSERT INTO ContributionCampaigns (Id, ChurchId, Name, [Description], TargetAmount, Currency, StartDate, EndDate, [Status], FundId, CreatedAt, IsDeleted) VALUES
(@Campaign1, @ParentChurchId, N'Bâtissons ensemble 2026', N'Campagne pour la construction du nouveau bâtiment de l''église', 50000000.00, 'XAF', '2026-01-01', '2026-12-31', 2, @FundBuilding, @Now, 0);

-- ============================================================================
-- 8. CONTRIBUTIONS
-- ============================================================================
INSERT INTO Contributions (Id, ChurchId, ReferenceNumber, Amount, Currency, ContributionDate, [Type], [Status], Notes, MemberId, FundId, CampaignId, IsRecurring, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, 'CTR-2026-001', 150000.00, 'XAF', '2026-01-07', 1, 2, N'Dîme de janvier',           @Mbr01, @FundGeneral,  NULL,       0, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-002',  75000.00, 'XAF', '2026-01-07', 1, 2, N'Offrande du dimanche',      @Mbr05, @FundGeneral,  NULL,       0, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-003', 500000.00, 'XAF', '2026-01-15', 3, 2, N'Don pour la construction',   @Mbr06, @FundBuilding, @Campaign1, 0, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-004',  50000.00, 'XAF', '2026-02-04', 1, 2, N'Dîme de février',           @Mbr12, @FundGeneral,  NULL,       0, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-005', 200000.00, 'XAF', '2026-02-14', 4, 2, N'Contribution via mobile money',@Mbr09, @FundMissions, NULL,     0, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-006', 100000.00, 'XAF', '2026-03-02', 1, 2, N'Dîme de mars',              @Mbr02, @FundGeneral,  NULL,       1, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-007',1000000.00, 'XAF', '2026-03-20', 3, 2, N'Don spécial construction',   @Mbr01, @FundBuilding, @Campaign1, 0, @Now, 0);

-- ============================================================================
-- 9. PLEDGES
-- ============================================================================
INSERT INTO Pledges (Id, ChurchId, MemberId, FundId, CampaignId, PledgedAmount, PaidAmount, Currency, StartDate, EndDate, [Status], Notes, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Mbr01, @FundBuilding, @Campaign1, 5000000.00, 1500000.00, 'XAF', '2026-01-01', '2026-12-31', 1, N'Engagement annuel pour le bâtiment', @Now, 0),
(NEWID(), @ParentChurchId, @Mbr05, @FundBuilding, @Campaign1, 2000000.00,  500000.00, 'XAF', '2026-01-01', '2026-12-31', 1, N'Promesse de contribution',           @Now, 0),
(NEWID(), @ParentChurchId, @Mbr06, @FundBuilding, @Campaign1, 3000000.00,  500000.00, 'XAF', '2026-01-01', '2026-12-31', 1, N'Engagement trimestriel',             @Now, 0);

-- ============================================================================
-- 10. BANK ACCOUNTS
-- ============================================================================
DECLARE @BankAcct1 UNIQUEIDENTIFIER = 'E0000003-0000-0000-0000-000000000001';
DECLARE @BankAcct2 UNIQUEIDENTIFIER = 'E0000003-0000-0000-0000-000000000002';

INSERT INTO BankAccounts (Id, ChurchId, AccountName, AccountNumber, BankName, BranchName, AccountType, Currency, CurrentBalance, IsActive, IsDefault, CreatedAt, IsDeleted) VALUES
(@BankAcct1, @ParentChurchId, N'Compte courant principal', '10001-23456-78900', N'Afriland First Bank',  N'Agence Douala Centre', 3, 'XAF', 12500000.00, 1, 1, @Now, 0),
(@BankAcct2, @ParentChurchId, N'Compte épargne projet',    '20001-98765-43210', N'Société Générale Cameroun', N'Agence Akwa',    2, 'XAF',  8750000.00, 1, 0, @Now, 0);

-- ============================================================================
-- 11. ACCOUNT TRANSACTIONS
-- ============================================================================
INSERT INTO AccountTransactions (Id, ChurchId, BankAccountId, [Type], Amount, Currency, TransactionDate, [Description], RunningBalance, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @BankAcct1, 1, 2075000.00, 'XAF', '2026-01-31', N'Versement dîmes et offrandes — janvier 2026',   12500000.00, @Now, 0),
(NEWID(), @ParentChurchId, @BankAcct2, 1, 1500000.00, 'XAF', '2026-02-28', N'Transfert vers compte épargne projet',           8750000.00,  @Now, 0),
(NEWID(), @ParentChurchId, @BankAcct1, 2,  350000.00, 'XAF', '2026-03-05', N'Paiement facture électricité — mars 2026',      12150000.00, @Now, 0);

-- ============================================================================
-- 12. EXPENSE CATEGORIES
-- ============================================================================
DECLARE @ExpCatUtilities UNIQUEIDENTIFIER = 'E0000004-0000-0000-0000-000000000001';
DECLARE @ExpCatSalaries  UNIQUEIDENTIFIER = 'E0000004-0000-0000-0000-000000000002';
DECLARE @ExpCatEvents    UNIQUEIDENTIFIER = 'E0000004-0000-0000-0000-000000000003';
DECLARE @ExpCatSupplies  UNIQUEIDENTIFIER = 'E0000004-0000-0000-0000-000000000004';

INSERT INTO ExpenseCategories (Id, ChurchId, Name, [Description], Color, CreatedAt, IsDeleted) VALUES
(@ExpCatUtilities, @ParentChurchId, N'Services publics',     N'Électricité, eau, internet',          '#FF5722', @Now, 0),
(@ExpCatSalaries,  @ParentChurchId, N'Salaires et primes',   N'Rémunération du personnel',           '#2196F3', @Now, 0),
(@ExpCatEvents,    @ParentChurchId, N'Événements',           N'Organisation des événements',          '#4CAF50', @Now, 0),
(@ExpCatSupplies,  @ParentChurchId, N'Fournitures de bureau', N'Papeterie, encre, matériel divers',  '#9C27B0', @Now, 0);

-- ============================================================================
-- 13. EXPENSES
-- ============================================================================
INSERT INTO Expenses (Id, ChurchId, Title, [Description], Amount, Currency, ExpenseDate, [Status], PaymentMethod, VendorName, CategoryId, BankAccountId, SubmittedByMemberId, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, N'Facture électricité mars',     N'ENEO — consommation mars 2026',       350000.00, 'XAF', '2026-03-05', 5, 3, N'ENEO Cameroun',           @ExpCatUtilities, @BankAcct1, @Mbr05, @Now, 0),
(NEWID(), @ParentChurchId, N'Salaire gardien mars',         N'Rémunération mensuelle du gardien',   120000.00, 'XAF', '2026-03-28', 5, 1, NULL,                       @ExpCatSalaries,  @BankAcct1, @Mbr05, @Now, 0),
(NEWID(), @ParentChurchId, N'Fournitures culte de Pâques', N'Décoration, programmes imprimés',     85000.00,  'XAF', '2026-04-01', 3, 1, N'Imprimerie du Littoral',  @ExpCatEvents,    NULL,       @Mbr04, @Now, 0),
(NEWID(), @ParentChurchId, N'Facture internet avril',       N'Abonnement fibre optique',            45000.00,  'XAF', '2026-04-05', 2, 3, N'CAMTEL',                  @ExpCatUtilities, NULL,       @Mbr03, @Now, 0);

-- ============================================================================
-- 14. BUDGETS & BUDGET LINES
-- ============================================================================
DECLARE @Budget2026 UNIQUEIDENTIFIER = 'E0000005-0000-0000-0000-000000000001';

INSERT INTO Budgets (Id, ChurchId, Name, [Year], StartDate, EndDate, Currency, TotalAmount, [Status], Notes, CreatedAt, IsDeleted) VALUES
(@Budget2026, @ParentChurchId, N'Budget annuel 2026', 2026, '2026-01-01', '2026-12-31', 'XAF', 15000000.00, 2, N'Budget approuvé en assemblée générale', @Now, 0);

INSERT INTO BudgetLines (Id, ChurchId, BudgetId, CategoryId, Name, AllocatedAmount, SpentAmount, Notes, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Budget2026, @ExpCatUtilities, N'Services publics annuels',   4200000.00, 395000.00, N'Électricité + eau + internet', @Now, 0),
(NEWID(), @ParentChurchId, @Budget2026, @ExpCatSalaries,  N'Masse salariale',            6000000.00, 120000.00, N'Gardien + secrétaire',         @Now, 0),
(NEWID(), @ParentChurchId, @Budget2026, @ExpCatEvents,    N'Événements et célébrations', 3000000.00,  85000.00, N'Pâques, Noël, conventions',    @Now, 0),
(NEWID(), @ParentChurchId, @Budget2026, @ExpCatSupplies,  N'Fournitures diverses',       1800000.00,       0,   N'Bureau et entretien',          @Now, 0);

-- ============================================================================
-- 15. CHURCH EVENTS
-- ============================================================================
DECLARE @Event1 UNIQUEIDENTIFIER = 'F0000001-0000-0000-0000-000000000001';
DECLARE @Event2 UNIQUEIDENTIFIER = 'F0000001-0000-0000-0000-000000000002';
DECLARE @Event3 UNIQUEIDENTIFIER = 'F0000001-0000-0000-0000-000000000003';

INSERT INTO ChurchEvents (Id, ChurchId, Title, [Description], [Type], [Status], StartDateTime, EndDateTime, [Location], RequiresRegistration, MaxAttendees, QrCodeValue, IsRecurring, RecurrenceFrequency, CreatedAt, IsDeleted) VALUES
(@Event1, @ParentChurchId, N'Culte dominical',             N'Célébration du dimanche avec louange et prédication', 0, 1, '2026-04-13 09:00:00', '2026-04-13 12:00:00', N'Temple principal — Douala', 0, 500, 'QR-EVT-001', 1, 1, @Now, 0),
(@Event2, @ParentChurchId, N'Conférence de Pâques 2026',   N'Trois jours de retraite spirituelle avec orateurs invités', 1, 1, '2026-04-18 08:00:00', '2026-04-20 17:00:00', N'Centre de conférences Bonapriso', 1, 200, 'QR-EVT-002', 0, NULL, @Now, 0),
(@Event3, @ParentChurchId, N'Soirée de louange et prière', N'Veillée de prière avec l''équipe de louange',        8, 3, '2026-03-28 19:00:00', '2026-03-29 05:00:00', N'Temple principal — Douala', 0, NULL, 'QR-EVT-003', 0, NULL, @Now, 0);

-- ============================================================================
-- 16. EVENT REGISTRATIONS
-- ============================================================================
INSERT INTO EventRegistrations (Id, ChurchId, EventId, MemberId, [Status], RegistrationCode, IsPaid, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Event2, @Mbr01, 1, 'REG-PAQUES-001', 1, @Now, 0),
(NEWID(), @ParentChurchId, @Event2, @Mbr04, 1, 'REG-PAQUES-002', 1, @Now, 0),
(NEWID(), @ParentChurchId, @Event2, @Mbr09, 1, 'REG-PAQUES-003', 1, @Now, 0),
(NEWID(), @ParentChurchId, @Event2, @Mbr12, 0, 'REG-PAQUES-004', 0, @Now, 0);

-- ============================================================================
-- 17. EVENT ATTENDANCE
-- ============================================================================
INSERT INTO EventAttendances (Id, ChurchId, EventId, MemberId, AttendanceDate, [Status], RecordedByQr, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Event3, @Mbr01, '2026-03-28', 0, 0, @Now, 0),
(NEWID(), @ParentChurchId, @Event3, @Mbr04, '2026-03-28', 0, 0, @Now, 0),
(NEWID(), @ParentChurchId, @Event3, @Mbr05, '2026-03-28', 0, 1, @Now, 0),
(NEWID(), @ParentChurchId, @Event3, @Mbr08, '2026-03-28', 0, 0, @Now, 0),
(NEWID(), @ParentChurchId, @Event3, @Mbr09, '2026-03-28', 2, 0, @Now, 0),
(NEWID(), @ParentChurchId, @Event3, @Mbr12, '2026-03-28', 1, 0, @Now, 0);

-- ============================================================================
-- 18. SUNDAY SCHOOL
-- ============================================================================
DECLARE @SSClass1 UNIQUEIDENTIFIER = 'F0000002-0000-0000-0000-000000000001';
DECLARE @SSClass2 UNIQUEIDENTIFIER = 'F0000002-0000-0000-0000-000000000002';

INSERT INTO SundaySchoolClasses (Id, ChurchId, Name, [Description], [Level], IsActive, TeacherId, MinAge, MaxAge, MaxCapacity, CreatedAt, IsDeleted) VALUES
(@SSClass1, @ParentChurchId, N'Les Étoiles du Matin',   N'Classe pour enfants de 6 à 10 ans',  2, 1, @Mbr08, 6,  10, 30, @Now, 0),
(@SSClass2, @ParentChurchId, N'Les Flambeaux de la Foi', N'Classe pour adolescents 11 à 17 ans',4, 1, @Mbr08, 11, 17, 25, @Now, 0);

DECLARE @SSLesson1 UNIQUEIDENTIFIER = 'F0000002-1000-0000-0000-000000000001';
INSERT INTO SundaySchoolLessons (Id, ChurchId, ClassId, Title, [Description], LessonDate, DurationMinutes, CreatedAt, IsDeleted) VALUES
(@SSLesson1, @ParentChurchId, @SSClass1, N'L''histoire de Noé et l''arche',   N'Genèse 6-9 — La fidélité de Dieu dans l''épreuve', '2026-04-06', 45, @Now, 0),
(NEWID(),    @ParentChurchId, @SSClass1, N'David et Goliath',                  N'1 Samuel 17 — Le courage face à l''adversité',      '2026-04-13', 45, @Now, 0),
(NEWID(),    @ParentChurchId, @SSClass2, N'Les Béatitudes',                    N'Matthieu 5 — Les valeurs du Royaume',                '2026-04-06', 60, @Now, 0);

INSERT INTO SundaySchoolEnrollments (Id, ChurchId, ClassId, MemberId, EnrolledDate, [Status], CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @SSClass1, @Mbr12, '2026-01-15', 0, @Now, 0),
(NEWID(), @ParentChurchId, @SSClass1, @Mbr13, '2026-01-15', 0, @Now, 0),
(NEWID(), @ParentChurchId, @SSClass2, @Mbr07, '2026-02-01', 0, @Now, 0);

INSERT INTO SundaySchoolAttendances (Id, ChurchId, LessonId, MemberId, [Status], CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @SSLesson1, @Mbr12, 0, @Now, 0),
(NEWID(), @ParentChurchId, @SSLesson1, @Mbr13, 0, @Now, 0);

-- ============================================================================
-- 19. GROWTH SCHOOL
-- ============================================================================
DECLARE @GSCourse1 UNIQUEIDENTIFIER = 'F0000003-0000-0000-0000-000000000001';

INSERT INTO GrowthSchoolCourses (Id, ChurchId, Name, [Description], [Level], IsActive, InstructorId, DurationWeeks, MaxCapacity, CreatedAt, IsDeleted) VALUES
(@GSCourse1, @ParentChurchId, N'Fondements de la foi chrétienne', N'Cours de base pour les nouveaux convertis — découverte des vérités essentielles', 0, 1, @Mbr08, 12, 20, @Now, 0);

DECLARE @GSSession1 UNIQUEIDENTIFIER = 'F0000003-1000-0000-0000-000000000001';
INSERT INTO GrowthSchoolSessions (Id, ChurchId, CourseId, Title, [Description], SessionDate, DurationMinutes, CreatedAt, IsDeleted) VALUES
(@GSSession1, @ParentChurchId, @GSCourse1, N'Qui est Dieu ?',           N'Leçon 1 — Les attributs de Dieu',        '2026-04-05', 90, @Now, 0),
(NEWID(),     @ParentChurchId, @GSCourse1, N'L''autorité de la Bible',  N'Leçon 2 — Pourquoi la Bible est fiable', '2026-04-12', 90, @Now, 0);

INSERT INTO GrowthSchoolEnrollments (Id, ChurchId, CourseId, MemberId, EnrolledDate, [Status], CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @GSCourse1, @Mbr12, '2026-03-15', 0, @Now, 0),
(NEWID(), @ParentChurchId, @GSCourse1, @Mbr13, '2026-03-15', 0, @Now, 0);

INSERT INTO GrowthSchoolAttendances (Id, ChurchId, SessionId, MemberId, [Status], CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @GSSession1, @Mbr12, 0, @Now, 0),
(NEWID(), @ParentChurchId, @GSSession1, @Mbr13, 0, @Now, 0);

-- ============================================================================
-- 20. DEPARTMENTS
-- ============================================================================
DECLARE @DeptWorship UNIQUEIDENTIFIER = 'F0000004-0000-0000-0000-000000000001';
DECLARE @DeptYouth UNIQUEIDENTIFIER   = 'F0000004-0000-0000-0000-000000000002';
DECLARE @DeptWomen UNIQUEIDENTIFIER   = 'F0000004-0000-0000-0000-000000000003';

INSERT INTO Departments (Id, ChurchId, Name, [Description], LeaderId, Color, IsActive, CreatedAt, IsDeleted) VALUES
(@DeptWorship, @ParentChurchId, N'Département de louange',   N'Chorales, musiciens et équipe technique de sonorisation', @Mbr06, '#1565C0', 1, @Now, 0),
(@DeptYouth,   @ParentChurchId, N'Département de la jeunesse',N'Activités et encadrement des jeunes de 18 à 35 ans',     @Mbr06, '#FF9800', 1, @Now, 0),
(@DeptWomen,   @ParentChurchId, N'Département des femmes',    N'Groupe Déborah — fraternité et entraide féminine',        @Mbr02, '#E91E63', 1, @Now, 0);

INSERT INTO DepartmentMembers (Id, ChurchId, DepartmentId, MemberId, [Role], JoinedDate, IsActive, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @DeptWorship, @Mbr06, 0, '2020-01-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWorship, @Mbr10, 4, '2020-06-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWorship, @Mbr12, 4, '2021-01-15', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptYouth,   @Mbr06, 0, '2019-09-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptYouth,   @Mbr13, 4, '2022-03-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWomen,   @Mbr02, 0, '2018-01-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWomen,   @Mbr04, 3, '2018-06-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWomen,   @Mbr07, 2, '2019-01-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWomen,   @Mbr15, 4, '2018-03-01', 1, @Now, 0);

INSERT INTO DepartmentTransactions (Id, ChurchId, DepartmentId, [Type], Amount, [Description], [Date], RecordedByMemberId, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @DeptWorship, 0, 250000.00, N'Cotisations membres — trimestre 1', '2026-03-31', @Mbr07, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWorship, 1, 180000.00, N'Achat de cordes de guitare et microphones', '2026-03-15', @Mbr07, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWomen,   0, 175000.00, N'Cotisations mensuelles février',    '2026-02-28', @Mbr07, @Now, 0);

-- ============================================================================
-- 21. MESSAGING & NOTIFICATIONS
-- ============================================================================
DECLARE @MsgCampaign1 UNIQUEIDENTIFIER = 'F0000005-0000-0000-0000-000000000001';

INSERT INTO MessageCampaigns (Id, ChurchId, Title, Body, Channel, [Status], ScheduledAt, SentAt, SentByMemberId, RecipientCount, DeliveredCount, FailedCount, CreatedAt, IsDeleted) VALUES
(@MsgCampaign1, @ParentChurchId, N'Invitation Conférence de Pâques', N'Chers frères et sœurs, vous êtes cordialement invités à la conférence de Pâques du 18 au 20 avril 2026. Inscription obligatoire. Que Dieu vous bénisse !', 0, 3, '2026-04-01 08:00:00', '2026-04-01 08:05:00', @Mbr04, 12, 11, 1, @Now, 0);

INSERT INTO MessageRecipients (Id, ChurchId, CampaignId, MemberId, [Status], SentAt, DeliveredAt, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @MsgCampaign1, @Mbr01, 2, '2026-04-01 08:05:00', '2026-04-01 08:05:02', @Now, 0),
(NEWID(), @ParentChurchId, @MsgCampaign1, @Mbr05, 2, '2026-04-01 08:05:00', '2026-04-01 08:05:03', @Now, 0),
(NEWID(), @ParentChurchId, @MsgCampaign1, @Mbr09, 3, '2026-04-01 08:05:00', NULL,                   @Now, 0);

-- ============================================================================
-- 22. APPOINTMENTS
-- ============================================================================
INSERT INTO Appointments (Id, ChurchId, MemberId, ResponsibleMemberId, Subject, [Description], [Status], MeetingType, RequestedAt, ScheduledAt, [Location], ReminderSent10Min, ReminderSent5Min, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Mbr12, @Mbr01, N'Conseil pastoral',            N'Demande de conseils pour orientation spirituelle',           2, 0, '2026-04-05 10:00:00', '2026-04-15 14:00:00', N'Bureau du pasteur', 0, 0, @Now, 0),
(NEWID(), @ParentChurchId, @Mbr13, @Mbr01, N'Préparation au baptême',      N'Entretien avant le baptême d''eau prévu en juin',            1, 0, '2026-04-10 09:00:00', '2026-04-20 10:00:00', N'Bureau du pasteur', 0, 0, @Now, 0),
(NEWID(), @ParentChurchId, @Mbr07, @Mbr05, N'Questions sur les finances',  N'Besoin de clarification sur le relevé de contributions',     4, 1, '2026-03-20 11:00:00', '2026-03-25 15:00:00', NULL,                 1, 1, @Now, 0);

-- ============================================================================
-- 23. NOTIFICATIONS
-- ============================================================================
INSERT INTO Notifications (Id, ChurchId, UserId, Title, Body, [Type], IsRead, RelatedEntityType, ActionUrl, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @UserMember1,    N'Conférence de Pâques',            N'N''oubliez pas de vous inscrire à la conférence du 18-20 avril !', 4, 0, 'ChurchEvent', '/events', @Now, 0),
(NEWID(), @ParentChurchId, @UserTreasurer,  N'Nouveau don reçu',                N'Un don de 1 000 000 XAF a été enregistré pour le fonds de construction.', 1, 1, 'Contribution', '/finance/contributions', @Now, 0),
(NEWID(), @ParentChurchId, @UserChurchAdmin,N'Ticket de support ouvert',        N'Célestin Mbappé a ouvert un ticket concernant le réseau Wi-Fi.',  0, 0, 'SupportTicket', '/it/tickets', @Now, 0);

-- ============================================================================
-- 24. SECRETARIAT: DOCUMENTS, CERTIFICATES, BAPTISM & MARRIAGE RECORDS
-- ============================================================================
INSERT INTO Documents (Id, ChurchId, Title, [FileName], FileUrl, FileSize, ContentType, [Type], MemberId, UploadedByMemberId, Notes, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, N'Procès-verbal AG janvier 2026', 'pv-ag-jan-2026.pdf', '/docs/pv-ag-jan-2026.pdf', 524288, 'application/pdf', 5, NULL,  @Mbr04, N'Assemblée générale ordinaire', @Now, 0),
(NEWID(), @ParentChurchId, N'Règlement intérieur',           'reglement-interieur.pdf', '/docs/reglement-interieur.pdf', 1048576, 'application/pdf', 0, NULL, @Mbr04, N'Version mise à jour 2026',     @Now, 0);

DECLARE @CertBaptism1 UNIQUEIDENTIFIER = NEWID();
INSERT INTO Certificates (Id, ChurchId, [Type], CertificateNumber, MemberId, IssuedDate, IssuedByMemberId, CreatedAt, IsDeleted) VALUES
(@CertBaptism1, @ParentChurchId, 0, 'CERT-BAPT-2026-001', @Mbr12, '2019-09-22', @Mbr01, @Now, 0),
(NEWID(),       @ParentChurchId, 2, 'CERT-MBR-2026-001',  @Mbr01, '2010-01-15', NULL,    @Now, 0);

INSERT INTO BaptismRecords (Id, ChurchId, MemberId, BaptismDate, OfficiantMemberId, [Location], Notes, CertificateId, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Mbr12, '2019-09-22', @Mbr01, N'Rivière Wouri — Douala', N'Baptême par immersion complète', @CertBaptism1, @Now, 0);

DECLARE @CertMarriage1 UNIQUEIDENTIFIER = NEWID();
INSERT INTO Certificates (Id, ChurchId, [Type], CertificateNumber, MemberId, IssuedDate, IssuedByMemberId, CreatedAt, IsDeleted) VALUES
(@CertMarriage1, @ParentChurchId, 1, 'CERT-MAR-2026-001', @Mbr01, '2005-08-20', NULL, @Now, 0);

INSERT INTO MarriageRecords (Id, ChurchId, Spouse1MemberId, Spouse2MemberId, MarriageDate, OfficiantMemberId, [Location], Notes, CertificateId, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Mbr01, @Mbr15, '2005-08-20', NULL, N'Église Évangélique de la Grâce — Douala', N'Cérémonie religieuse et bénédiction nuptiale', @CertMarriage1, @Now, 0);

-- ============================================================================
-- 25. EVANGELISM
-- ============================================================================
DECLARE @EvangCamp1 UNIQUEIDENTIFIER = 'F0000006-0000-0000-0000-000000000001';
DECLARE @EvangTeam1 UNIQUEIDENTIFIER = 'F0000006-1000-0000-0000-000000000001';
DECLARE @EvangContact1 UNIQUEIDENTIFIER = NEWID();

INSERT INTO EvangelismCampaigns (Id, ChurchId, Name, [Description], StartDate, EndDate, [Status], GoalContacts, LeaderMemberId, Notes, CreatedAt, IsDeleted) VALUES
(@EvangCamp1, @ParentChurchId, N'Opération Moisson 2026', N'Grande campagne d''évangélisation de quartier — porte à porte et distributions de tracts', '2026-05-01', '2026-05-31', 1, 200, @Mbr09, N'Budget de 500 000 XAF approuvé', @Now, 0);

INSERT INTO EvangelismTeams (Id, ChurchId, CampaignId, Name, LeaderMemberId, CreatedAt, IsDeleted) VALUES
(@EvangTeam1, @ParentChurchId, @EvangCamp1, N'Équipe Alpha — Quartier Akwa', @Mbr09, @Now, 0);

INSERT INTO EvangelismTeamMembers (Id, ChurchId, TeamId, MemberId, JoinedDate, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @EvangTeam1, @Mbr09, '2026-04-20', @Now, 0),
(NEWID(), @ParentChurchId, @EvangTeam1, @Mbr13, '2026-04-20', @Now, 0),
(NEWID(), @ParentChurchId, @EvangTeam1, @Mbr12, '2026-04-22', @Now, 0);

INSERT INTO EvangelismContacts (Id, ChurchId, CampaignId, TeamId, FirstName, LastName, Phone, [Address], [Status], AssignedToMemberId, Notes, CreatedAt, IsDeleted) VALUES
(@EvangContact1, @ParentChurchId, @EvangCamp1, @EvangTeam1, N'Hervé',   N'Toko',     '+237 670 999 111', N'Quartier Akwa, Rue 1020',   0, @Mbr09, N'Rencontré devant la boulangerie',     @Now, 0),
(NEWID(),        @ParentChurchId, @EvangCamp1, @EvangTeam1, N'Clémentine', N'Ndongo', '+237 670 999 222', N'Quartier Akwa, Rue 1035',   1, @Mbr13, N'Intéressée — a accepté une Bible',     @Now, 0),
(NEWID(),        @ParentChurchId, @EvangCamp1, @EvangTeam1, N'Désiré',  N'Mbiakop',  '+237 670 999 333', N'Quartier Bépanda, Carrefour', 2, @Mbr12, N'A assisté au culte du 13 avril 2026', @Now, 0);

INSERT INTO EvangelismFollowUps (Id, ChurchId, ContactId, Method, FollowUpDate, Notes, ConductedByMemberId, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @EvangContact1, 0, '2026-04-25', N'Visite à domicile — a partagé l''Évangile de Jean', @Mbr09, @Now, 0);

-- ============================================================================
-- 26. MULTIMEDIA
-- ============================================================================
DECLARE @Media1 UNIQUEIDENTIFIER = NEWID();

INSERT INTO MediaContents (Id, ChurchId, Title, [Description], ContentType, [Status], AccessType, Price, FileUrl, ThumbnailUrl, DurationSeconds, Tags, DownloadCount, ViewCount, AuthorMemberId, PublishedAt, CreatedAt, IsDeleted) VALUES
(@Media1,  @ParentChurchId, N'Prédication — La puissance de la résurrection', N'Message de Pâques 2026 par le Pasteur Éric Nguéma',  1, 1, 0, NULL,      '/media/predication-paques-2026.mp4',  '/media/thumb-paques-2026.jpg', 3600, N'prédication,Pâques,résurrection', 45, 230, @Mbr01, '2026-04-01', @Now, 0),
(NEWID(),  @ParentChurchId, N'Album — Cantiques de la Grâce vol. 3',          N'Nouvel album de la chorale avec 12 chants originaux', 0, 1, 1, 2500.00, '/media/cantiques-grace-vol3.zip',     '/media/thumb-cantiques-v3.jpg', NULL, N'musique,chorale,louange',          12,  89, @Mbr10, '2026-03-15', @Now, 0);

INSERT INTO MediaPurchases (Id, ChurchId, ContentId, MemberId, Amount, [Status], PaymentReference, PaidAt, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Media1, @Mbr12, 0, 1, NULL, @Now, @Now, 0);

INSERT INTO MediaPromotions (Id, ChurchId, ContentId, Title, [Description], Code, DiscountPercent, StartDate, EndDate, IsActive, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, NULL, N'Promo Pâques -20%', N'20% de réduction sur tous les contenus payants pour Pâques', 'PAQUES2026', 20.00, '2026-04-10', '2026-04-25', 1, @Now, 0);

-- ============================================================================
-- 27. LOGISTICS: INVENTORY & VEHICLES
-- ============================================================================
DECLARE @InvItem1 UNIQUEIDENTIFIER = 'F0000007-0000-0000-0000-000000000001';
DECLARE @InvItem2 UNIQUEIDENTIFIER = 'F0000007-0000-0000-0000-000000000002';

INSERT INTO InventoryItems (Id, ChurchId, Name, [Description], Category, Quantity, Unit, MinQuantity, [Location], [Status], CreatedAt, IsDeleted) VALUES
(@InvItem1, @ParentChurchId, N'Chaises pliantes',     N'Chaises en plastique blanc pour événements',       N'Mobilier',    200, N'pièces', 50, N'Entrepôt principal',  0, @Now, 0),
(@InvItem2, @ParentChurchId, N'Microphones sans fil',  N'Shure SM58 — microphones de scène',               N'Sonorisation', 6,  N'pièces',  2, N'Salle technique',     0, @Now, 0);

INSERT INTO InventoryTransactions (Id, ChurchId, ItemId, [Type], QuantityChange, QuantityAfter, TransactionDate, RecordedByMemberId, Notes, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @InvItem1, 0, 200, 200, '2026-01-10', @Mbr11, N'Stock initial de chaises',           @Now, 0),
(NEWID(), @ParentChurchId, @InvItem1, 1, -50, 150, '2026-04-10', @Mbr11, N'Prêt de 50 chaises pour la conférence', @Now, 0),
(NEWID(), @ParentChurchId, @InvItem2, 0, 6,   6,   '2026-01-10', @Mbr11, N'Achat de 6 microphones Shure',       @Now, 0);

DECLARE @Vehicle1 UNIQUEIDENTIFIER = 'F0000007-2000-0000-0000-000000000001';

INSERT INTO Vehicles (Id, ChurchId, Make, Model, [Year], PlateNumber, Capacity, [Status], Color, CreatedAt, IsDeleted) VALUES
(@Vehicle1, @ParentChurchId, 'Toyota', 'HiAce', 2022, 'LT-3456-A', 15, 0, N'Blanc', @Now, 0);

INSERT INTO VehicleBookings (Id, ChurchId, VehicleId, DriverMemberId, Purpose, StartDateTime, EndDateTime, [Status], ApprovedByMemberId, ApprovedAt, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Vehicle1, @Mbr11, N'Transport des fidèles pour la conférence de Pâques', '2026-04-18 07:00:00', '2026-04-20 18:00:00', 1, @Mbr06, @Now, @Now, 0);

-- ============================================================================
-- 28. IT MANAGEMENT: TICKETS, INTEGRATIONS, LOGS
-- ============================================================================
DECLARE @Ticket1 UNIQUEIDENTIFIER = NEWID();

INSERT INTO SupportTickets (Id, ChurchId, Title, [Description], Category, [Priority], [Status], SubmittedByUserId, AssignedToUserId, CreatedAt, IsDeleted) VALUES
(@Ticket1, @ParentChurchId, N'Problème de réseau Wi-Fi au temple',  N'Le Wi-Fi ne fonctionne pas correctement depuis dimanche dernier. Les fidèles ne peuvent pas se connecter.', 0, 2, 1, @UserChurchAdmin, @UserITManager, @Now, 0),
(NEWID(),  @ParentChurchId, N'Demande d''ajout du module de dons en ligne', N'Il serait utile d''intégrer un système de paiement en ligne pour les contributions.', 2, 1, 0, @UserTreasurer, NULL, @Now, 0);

INSERT INTO SupportTicketComments (Id, ChurchId, TicketId, AuthorId, Content, IsInternal, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Ticket1, @UserITManager, N'J''ai identifié le problème : le routeur principal doit être remplacé. Commande en cours.', 0, @Now, 0);

INSERT INTO IntegrationConfigs (Id, ChurchId, [Service], IsEnabled, WebhookUrl, IsHealthy, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, 3, 1, 'https://hooks.eeg-grace.org/twilio', 1, @Now, 0),
(NEWID(), @ParentChurchId, 4, 1, 'https://hooks.eeg-grace.org/sendgrid', 1, @Now, 0);

INSERT INTO SystemLogs (Id, ChurchId, [Action], EntityType, Details, UserId, [Level], CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, 'UserLogin',       'AppUser',      N'Connexion réussie depuis 192.168.1.45',                              @UserChurchAdmin, 'Info',    @Now, 0),
(NEWID(), @ParentChurchId, 'MemberCreated',   'Member',       N'Nouveau membre Émmanuel Tchinda ajouté au registre',                 @UserSecretary,   'Info',    @Now, 0),
(NEWID(), @ParentChurchId, 'ContributionAdded','Contribution', N'Contribution de 1 000 000 XAF enregistrée pour le fonds construction',@UserTreasurer,  'Info',    @Now, 0),
(NEWID(), @ParentChurchId, 'BackupFailed',    'System',       N'Échec de la sauvegarde automatique — espace disque insuffisant',     NULL,             'Error',   @Now, 0);

-- ============================================================================
-- 29. SUBSCRIPTIONS, INVOICES, SMS CREDITS
-- ============================================================================
DECLARE @Sub1 UNIQUEIDENTIFIER = 'F0000008-0000-0000-0000-000000000001';

INSERT INTO Subscriptions (Id, ChurchId, [Plan], [Status], BillingCycle, Amount, Currency, AutoRenew, StartDate, EndDate, NextBillingDate, PaymentMethod, CreatedAt, IsDeleted) VALUES
(@Sub1, @ParentChurchId, 4, 1, 1, 75000.00, 'XAF', 1, '2026-01-01', '2027-01-01', '2026-05-01', 4, @Now, 0);

INSERT INTO Invoices (Id, ChurchId, InvoiceNumber, [Description], Amount, Currency, [Status], DueDate, PaidAt, PaymentMethod, SubscriptionId, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, 'INV-2026-001', N'Abonnement Premium — janvier 2026', 75000.00, 'XAF', 2, '2026-01-15', '2026-01-10', 4, @Sub1, @Now, 0),
(NEWID(), @ParentChurchId, 'INV-2026-002', N'Abonnement Premium — février 2026', 75000.00, 'XAF', 2, '2026-02-15', '2026-02-12', 4, @Sub1, @Now, 0),
(NEWID(), @ParentChurchId, 'INV-2026-003', N'Abonnement Premium — mars 2026',    75000.00, 'XAF', 2, '2026-03-15', '2026-03-10', 4, @Sub1, @Now, 0),
(NEWID(), @ParentChurchId, 'INV-2026-004', N'Abonnement Premium — avril 2026',   75000.00, 'XAF', 1, '2026-04-15', NULL,          NULL, @Sub1, @Now, 0);

DECLARE @SmsCredit1 UNIQUEIDENTIFIER = 'F0000008-1000-0000-0000-000000000001';

INSERT INTO SmsCredits (Id, ChurchId, Balance, TotalPurchased, TotalConsumed, CreatedAt, IsDeleted) VALUES
(@SmsCredit1, @ParentChurchId, 488, 500, 12, @Now, 0);

INSERT INTO SmsCreditTransactions (Id, ChurchId, SmsCreditId, [Type], Amount, BalanceBefore, BalanceAfter, Reference, Notes, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @SmsCredit1, 0,  500, 0,   500, 'PAY-SMS-2026-001', N'Achat de 500 crédits SMS',          @Now, 0),
(NEWID(), @ParentChurchId, @SmsCredit1, 1, -12,  500, 488, 'CAMP-MSG-001',     N'Envoi campagne conférence de Pâques',@Now, 0);

-- ============================================================================
-- 30. CUSTOM FIELDS
-- ============================================================================
INSERT INTO CustomFields (Id, ChurchId, Name, Label, FieldType, IsRequired, DisplayOrder, Options, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, 'emergency_contact', N'Contact d''urgence',         0, 0, 1, NULL, @Now, 0),
(NEWID(), @ParentChurchId, 'allergies',         N'Allergies connues',           0, 0, 2, NULL, @Now, 0),
(NEWID(), @ParentChurchId, 'spiritual_gift',    N'Don spirituel principal',     4, 0, 3, N'["Enseignement","Prophétie","Guérison","Service","Sagesse","Évangélisation","Hospitalité"]', @Now, 0);

COMMIT TRANSACTION;
PRINT N'✓ Seed data inserted successfully — all tables populated.';
GO
