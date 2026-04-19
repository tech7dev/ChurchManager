-- ============================================================================
-- ChurchMS — Données de seed complètes (thème : République Démocratique du Congo)
-- Encodage UTF-8 avec accents français corrects (à, â, ç, è, é, ê, ë, î, ï, ô, ù, û, ü, œ)
-- ============================================================================
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET QUOTED_IDENTIFIER ON;

-- Idempotent : si déjà semé, sortir
IF EXISTS (SELECT 1 FROM Churches WHERE Id = 'A0000001-0000-0000-0000-000000000001')
BEGIN
    PRINT N'Données de seed déjà présentes — aucune action effectuée.';
    SET NOEXEC ON;
END

BEGIN TRANSACTION;

-- ============================================================================
-- 1. ÉGLISES (Siège à Kinshasa + antennes Lubumbashi et Goma)
-- ============================================================================
DECLARE @ParentChurchId UNIQUEIDENTIFIER = 'A0000001-0000-0000-0000-000000000001';
DECLARE @ChildChurch1Id UNIQUEIDENTIFIER = 'A0000001-0000-0000-0000-000000000002';
DECLARE @ChildChurch2Id UNIQUEIDENTIFIER = 'A0000001-0000-0000-0000-000000000003';
DECLARE @Now DATETIME = GETUTCDATE();

INSERT INTO Churches (Id, Name, Code, [Description], [Address], City, [State], Country, Phone, Email, Website, TimeZone, PrimaryCurrency, SecondaryCurrency, [Status], SubscriptionPlan, ParentChurchId, CreatedAt, IsDeleted)
VALUES
(@ParentChurchId, N'Église Évangélique de la Grâce — Kinshasa', 'EEG-KIN', N'Assemblée principale — siège central de l''Église Évangélique de la Grâce en République Démocratique du Congo', N'122 Avenue de la Libération, Commune de la Gombe', N'Kinshasa', N'Kinshasa', N'République Démocratique du Congo', '+243 811 234 567', 'contact@eeg-grace.cd', 'https://eeg-grace.cd', 'Africa/Kinshasa', 'CDF', 'USD', 1, 4, NULL, @Now, 0),
(@ChildChurch1Id, N'Église de la Grâce — Lubumbashi', 'EEG-FBM', N'Antenne de Lubumbashi, province du Haut-Katanga', N'45 Avenue Mobutu, Commune de Lubumbashi', N'Lubumbashi', N'Haut-Katanga', N'République Démocratique du Congo', '+243 822 345 678', 'lubumbashi@eeg-grace.cd', NULL, 'Africa/Lubumbashi', 'CDF', 'USD', 1, 3, @ParentChurchId, @Now, 0),
(@ChildChurch2Id, N'Église de la Grâce — Goma', 'EEG-GOM', N'Antenne de Goma, province du Nord-Kivu', N'18 Avenue du Lac, quartier Les Volcans', N'Goma', N'Nord-Kivu', N'République Démocratique du Congo', '+243 853 456 789', 'goma@eeg-grace.cd', NULL, 'Africa/Kinshasa', 'CDF', 'USD', 1, 2, @ParentChurchId, @Now, 0);

-- ============================================================================
-- 2. UTILISATEURS (plusieurs par rôle) — mot de passe commun : Password@123
-- ============================================================================
-- IDs des rôles dans AspNetRoles (déterministes via SHA-256 du nom)
DECLARE @RoleSuperAdmin   UNIQUEIDENTIFIER = '613A4DF9-4BFC-2D32-E134-AB222D849E50';
DECLARE @RoleCentralAdmin UNIQUEIDENTIFIER = 'D03E059B-FEA6-EC2D-E316-6A9526406A17';
DECLARE @RoleChurchAdmin  UNIQUEIDENTIFIER = '181FB183-29CF-98AB-9BF5-0E6586AFBB98';
DECLARE @RoleITManager    UNIQUEIDENTIFIER = '2ED18CE4-0485-3781-7E5A-9FA66C22C168';
DECLARE @RoleSecretary    UNIQUEIDENTIFIER = '6A999513-6810-6DD2-CB84-86DF1E1DB89B';
DECLARE @RoleTreasurer    UNIQUEIDENTIFIER = '73A7F390-B4FE-F40A-0179-9EF804DE8EC0';
DECLARE @RoleDeptHead     UNIQUEIDENTIFIER = '12C86DFE-37D2-A015-0200-3B37E5DDD6CF';
DECLARE @RoleDeptTreas    UNIQUEIDENTIFIER = '1449CAC4-DBD5-ECA9-306F-AE0631614598';
DECLARE @RoleTeacher      UNIQUEIDENTIFIER = '2EE5EE5E-E5C1-9880-9BE9-44EE7CAE917D';
DECLARE @RoleEvangLead    UNIQUEIDENTIFIER = '3565871D-A91B-1B69-044A-2A32F9AAC7B5';
DECLARE @RoleMultimedia   UNIQUEIDENTIFIER = 'F9893397-D7A8-E694-78D9-1C5C9E60D271';
DECLARE @RoleLogistics    UNIQUEIDENTIFIER = '6BF621D7-EA3B-C90E-EB5A-029AAC85EF24';
DECLARE @RoleMember       UNIQUEIDENTIFIER = 'B78F967C-501F-35E3-442B-35062A35620A';

-- GUIDs des utilisateurs
DECLARE @UCentralAdmin1 UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000001';
DECLARE @UCentralAdmin2 UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000002';
DECLARE @UChurchAdmin1  UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000003';
DECLARE @UChurchAdmin2  UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000004';
DECLARE @UITManager1    UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000005';
DECLARE @UITManager2    UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000006';
DECLARE @USecretary1    UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000007';
DECLARE @USecretary2    UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000008';
DECLARE @UTreasurer1    UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000009';
DECLARE @UTreasurer2    UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-00000000000A';
DECLARE @UDeptHead1     UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-00000000000B';
DECLARE @UDeptHead2     UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-00000000000C';
DECLARE @UDeptHead3     UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-00000000000D';
DECLARE @UDeptTreas1    UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-00000000000E';
DECLARE @UDeptTreas2    UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-00000000000F';
DECLARE @UTeacher1      UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000010';
DECLARE @UTeacher2      UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000011';
DECLARE @UTeacher3      UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000012';
DECLARE @UEvangLead1    UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000013';
DECLARE @UEvangLead2    UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000014';
DECLARE @UMultimedia1   UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000015';
DECLARE @UMultimedia2   UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000016';
DECLARE @ULogistics1    UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000017';
DECLARE @ULogistics2    UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000018';
DECLARE @UMember1       UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000019';
DECLARE @UMember2       UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-00000000001A';
DECLARE @UMember3       UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-00000000001B';
DECLARE @UMember4       UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-00000000001C';
DECLARE @UMember5       UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-00000000001D';
DECLARE @UMember6       UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-00000000001E';
DECLARE @UMember7       UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-00000000001F';
DECLARE @UMember8       UNIQUEIDENTIFIER = 'B0000001-0000-0000-0000-000000000020';

-- Hash de mot de passe pour "Password@123" (PBKDF2-HMAC-SHA256, 10 000 itérations — IdentityV3)
DECLARE @PwdHash NVARCHAR(MAX) = N'AQAAAAIAAYagAAAAEGqJUdD8R+fY9VMNu7z7PxImisnRHP4VMy7xQ1JkNfr2XbvMnO5VPx+L3VZ8Nh5oVQ==';
DECLARE @SecStamp NVARCHAR(MAX) = N'SEEDSTAMP000000000000000000000001';

INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, FirstName, LastName, ChurchId, IsActive, CreatedAt, IsDeleted)
VALUES
(@UCentralAdmin1, 'central1@eeg-grace.cd',     'CENTRAL1@EEG-GRACE.CD',     'central1@eeg-grace.cd',     'CENTRAL1@EEG-GRACE.CD',     1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 001', 0, 0, 1, 0, N'Patrice',     N'Tshilumba',     @ParentChurchId, 1, @Now, 0),
(@UCentralAdmin2, 'central2@eeg-grace.cd',     'CENTRAL2@EEG-GRACE.CD',     'central2@eeg-grace.cd',     'CENTRAL2@EEG-GRACE.CD',     1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 002', 0, 0, 1, 0, N'Bénédicte',   N'Kabongo',       @ParentChurchId, 1, @Now, 0),
(@UChurchAdmin1,  'admin.kin@eeg-grace.cd',    'ADMIN.KIN@EEG-GRACE.CD',    'admin.kin@eeg-grace.cd',    'ADMIN.KIN@EEG-GRACE.CD',    1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 003', 0, 0, 1, 0, N'Félicité',    N'Mwamba',        @ParentChurchId, 1, @Now, 0),
(@UChurchAdmin2,  'admin.lshi@eeg-grace.cd',   'ADMIN.LSHI@EEG-GRACE.CD',   'admin.lshi@eeg-grace.cd',   'ADMIN.LSHI@EEG-GRACE.CD',   1, @PwdHash, @SecStamp, NEWID(), '+243 822 100 004', 0, 0, 1, 0, N'André',       N'Ilunga',        @ChildChurch1Id, 1, @Now, 0),
(@UITManager1,    'it1@eeg-grace.cd',          'IT1@EEG-GRACE.CD',          'it1@eeg-grace.cd',          'IT1@EEG-GRACE.CD',          1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 005', 0, 0, 1, 0, N'Célestin',    N'Mukendi',       @ParentChurchId, 1, @Now, 0),
(@UITManager2,    'it2@eeg-grace.cd',          'IT2@EEG-GRACE.CD',          'it2@eeg-grace.cd',          'IT2@EEG-GRACE.CD',          1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 006', 0, 0, 1, 0, N'Jérémie',     N'Kasongo',       @ParentChurchId, 1, @Now, 0),
(@USecretary1,    'secretariat1@eeg-grace.cd', 'SECRETARIAT1@EEG-GRACE.CD', 'secretariat1@eeg-grace.cd', 'SECRETARIAT1@EEG-GRACE.CD', 1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 007', 0, 0, 1, 0, N'Béatrice',    N'Tshibanda',     @ParentChurchId, 1, @Now, 0),
(@USecretary2,    'secretariat2@eeg-grace.cd', 'SECRETARIAT2@EEG-GRACE.CD', 'secretariat2@eeg-grace.cd', 'SECRETARIAT2@EEG-GRACE.CD', 1, @PwdHash, @SecStamp, NEWID(), '+243 822 100 008', 0, 0, 1, 0, N'Thérèse',     N'Kalonji',       @ChildChurch1Id, 1, @Now, 0),
(@UTreasurer1,    'tresorier1@eeg-grace.cd',   'TRESORIER1@EEG-GRACE.CD',   'tresorier1@eeg-grace.cd',   'TRESORIER1@EEG-GRACE.CD',   1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 009', 0, 0, 1, 0, N'Étienne',     N'Mbuyi',         @ParentChurchId, 1, @Now, 0),
(@UTreasurer2,    'tresorier2@eeg-grace.cd',   'TRESORIER2@EEG-GRACE.CD',   'tresorier2@eeg-grace.cd',   'TRESORIER2@EEG-GRACE.CD',   1, @PwdHash, @SecStamp, NEWID(), '+243 853 100 010', 0, 0, 1, 0, N'Clémentine',  N'Ngoy',          @ChildChurch2Id, 1, @Now, 0),
(@UDeptHead1,     'dept.head1@eeg-grace.cd',   'DEPT.HEAD1@EEG-GRACE.CD',   'dept.head1@eeg-grace.cd',   'DEPT.HEAD1@EEG-GRACE.CD',   1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 011', 0, 0, 1, 0, N'François',    N'Kabila',        @ParentChurchId, 1, @Now, 0),
(@UDeptHead2,     'dept.head2@eeg-grace.cd',   'DEPT.HEAD2@EEG-GRACE.CD',   'dept.head2@eeg-grace.cd',   'DEPT.HEAD2@EEG-GRACE.CD',   1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 012', 0, 0, 1, 0, N'Gérard',      N'Mulumba',       @ParentChurchId, 1, @Now, 0),
(@UDeptHead3,     'dept.head3@eeg-grace.cd',   'DEPT.HEAD3@EEG-GRACE.CD',   'dept.head3@eeg-grace.cd',   'DEPT.HEAD3@EEG-GRACE.CD',   1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 013', 0, 0, 1, 0, N'Marie-Josée', N'Lumbala',       @ParentChurchId, 1, @Now, 0),
(@UDeptTreas1,    'dept.tres1@eeg-grace.cd',   'DEPT.TRES1@EEG-GRACE.CD',   'dept.tres1@eeg-grace.cd',   'DEPT.TRES1@EEG-GRACE.CD',   1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 014', 0, 0, 1, 0, N'Géraldine',   N'Tshiamala',     @ParentChurchId, 1, @Now, 0),
(@UDeptTreas2,    'dept.tres2@eeg-grace.cd',   'DEPT.TRES2@EEG-GRACE.CD',   'dept.tres2@eeg-grace.cd',   'DEPT.TRES2@EEG-GRACE.CD',   1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 015', 0, 0, 1, 0, N'Cécile',      N'Kayembe',       @ParentChurchId, 1, @Now, 0),
(@UTeacher1,      'enseignant1@eeg-grace.cd',  'ENSEIGNANT1@EEG-GRACE.CD',  'enseignant1@eeg-grace.cd',  'ENSEIGNANT1@EEG-GRACE.CD',  1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 016', 0, 0, 1, 0, N'Théophile',   N'Banza',         @ParentChurchId, 1, @Now, 0),
(@UTeacher2,      'enseignant2@eeg-grace.cd',  'ENSEIGNANT2@EEG-GRACE.CD',  'enseignant2@eeg-grace.cd',  'ENSEIGNANT2@EEG-GRACE.CD',  1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 017', 0, 0, 1, 0, N'Émilie',      N'Mukoko',        @ParentChurchId, 1, @Now, 0),
(@UTeacher3,      'enseignant3@eeg-grace.cd',  'ENSEIGNANT3@EEG-GRACE.CD',  'enseignant3@eeg-grace.cd',  'ENSEIGNANT3@EEG-GRACE.CD',  1, @PwdHash, @SecStamp, NEWID(), '+243 822 100 018', 0, 0, 1, 0, N'Noël',        N'Kasumba',       @ChildChurch1Id, 1, @Now, 0),
(@UEvangLead1,    'evangelisme1@eeg-grace.cd', 'EVANGELISME1@EEG-GRACE.CD', 'evangelisme1@eeg-grace.cd', 'EVANGELISME1@EEG-GRACE.CD', 1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 019', 0, 0, 1, 0, N'Moïse',       N'Tshisekedi',    @ParentChurchId, 1, @Now, 0),
(@UEvangLead2,    'evangelisme2@eeg-grace.cd', 'EVANGELISME2@EEG-GRACE.CD', 'evangelisme2@eeg-grace.cd', 'EVANGELISME2@EEG-GRACE.CD', 1, @PwdHash, @SecStamp, NEWID(), '+243 853 100 020', 0, 0, 1, 0, N'Daniel',      N'Kankonde',      @ChildChurch2Id, 1, @Now, 0),
(@UMultimedia1,   'multimedia1@eeg-grace.cd',  'MULTIMEDIA1@EEG-GRACE.CD',  'multimedia1@eeg-grace.cd',  'MULTIMEDIA1@EEG-GRACE.CD',  1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 021', 0, 0, 1, 0, N'Raphaël',     N'Bemba',         @ParentChurchId, 1, @Now, 0),
(@UMultimedia2,   'multimedia2@eeg-grace.cd',  'MULTIMEDIA2@EEG-GRACE.CD',  'multimedia2@eeg-grace.cd',  'MULTIMEDIA2@EEG-GRACE.CD',  1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 022', 0, 0, 1, 0, N'Stéphane',    N'Lukusa',        @ParentChurchId, 1, @Now, 0),
(@ULogistics1,    'logistique1@eeg-grace.cd',  'LOGISTIQUE1@EEG-GRACE.CD',  'logistique1@eeg-grace.cd',  'LOGISTIQUE1@EEG-GRACE.CD',  1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 023', 0, 0, 1, 0, N'Éric',        N'Lumumba',       @ParentChurchId, 1, @Now, 0),
(@ULogistics2,    'logistique2@eeg-grace.cd',  'LOGISTIQUE2@EEG-GRACE.CD',  'logistique2@eeg-grace.cd',  'LOGISTIQUE2@EEG-GRACE.CD',  1, @PwdHash, @SecStamp, NEWID(), '+243 822 100 024', 0, 0, 1, 0, N'Joseph',      N'Kasa-Vubu',     @ChildChurch1Id, 1, @Now, 0),
(@UMember1,       'membre1@eeg-grace.cd',      'MEMBRE1@EEG-GRACE.CD',      'membre1@eeg-grace.cd',      'MEMBRE1@EEG-GRACE.CD',      1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 025', 0, 0, 1, 0, N'Joséphine',   N'Mbuyi',         @ParentChurchId, 1, @Now, 0),
(@UMember2,       'membre2@eeg-grace.cd',      'MEMBRE2@EEG-GRACE.CD',      'membre2@eeg-grace.cd',      'MEMBRE2@EEG-GRACE.CD',      1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 026', 0, 0, 1, 0, N'Emmanuel',    N'Tshibola',      @ParentChurchId, 1, @Now, 0),
(@UMember3,       'membre3@eeg-grace.cd',      'MEMBRE3@EEG-GRACE.CD',      'membre3@eeg-grace.cd',      'MEMBRE3@EEG-GRACE.CD',      1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 027', 0, 0, 1, 0, N'Élisabeth',   N'Ntumba',        @ParentChurchId, 1, @Now, 0),
(@UMember4,       'membre4@eeg-grace.cd',      'MEMBRE4@EEG-GRACE.CD',      'membre4@eeg-grace.cd',      'MEMBRE4@EEG-GRACE.CD',      1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 028', 0, 0, 1, 0, N'Bernadette',  N'Mukeba',        @ParentChurchId, 1, @Now, 0),
(@UMember5,       'membre5@eeg-grace.cd',      'MEMBRE5@EEG-GRACE.CD',      'membre5@eeg-grace.cd',      'MEMBRE5@EEG-GRACE.CD',      1, @PwdHash, @SecStamp, NEWID(), '+243 811 100 029', 0, 0, 1, 0, N'Frédéric',    N'Mbombo',        @ParentChurchId, 1, @Now, 0),
(@UMember6,       'membre6@eeg-grace.cd',      'MEMBRE6@EEG-GRACE.CD',      'membre6@eeg-grace.cd',      'MEMBRE6@EEG-GRACE.CD',      1, @PwdHash, @SecStamp, NEWID(), '+243 822 100 030', 0, 0, 1, 0, N'Pascaline',   N'Mutombo',       @ChildChurch1Id, 1, @Now, 0),
(@UMember7,       'membre7@eeg-grace.cd',      'MEMBRE7@EEG-GRACE.CD',      'membre7@eeg-grace.cd',      'MEMBRE7@EEG-GRACE.CD',      1, @PwdHash, @SecStamp, NEWID(), '+243 853 100 031', 0, 0, 1, 0, N'Véronique',   N'Bilonda',       @ChildChurch2Id, 1, @Now, 0),
(@UMember8,       'membre8@eeg-grace.cd',      'MEMBRE8@EEG-GRACE.CD',      'membre8@eeg-grace.cd',      'MEMBRE8@EEG-GRACE.CD',      1, @PwdHash, @SecStamp, NEWID(), '+243 853 100 032', 0, 0, 1, 0, N'Léopold',     N'Mukwa',         @ChildChurch2Id, 1, @Now, 0);

-- Attribution des rôles
INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES
(@UCentralAdmin1, @RoleCentralAdmin),
(@UCentralAdmin2, @RoleCentralAdmin),
(@UChurchAdmin1,  @RoleChurchAdmin),
(@UChurchAdmin2,  @RoleChurchAdmin),
(@UITManager1,    @RoleITManager),
(@UITManager2,    @RoleITManager),
(@USecretary1,    @RoleSecretary),
(@USecretary2,    @RoleSecretary),
(@UTreasurer1,    @RoleTreasurer),
(@UTreasurer2,    @RoleTreasurer),
(@UDeptHead1,     @RoleDeptHead),
(@UDeptHead2,     @RoleDeptHead),
(@UDeptHead3,     @RoleDeptHead),
(@UDeptTreas1,    @RoleDeptTreas),
(@UDeptTreas2,    @RoleDeptTreas),
(@UTeacher1,      @RoleTeacher),
(@UTeacher2,      @RoleTeacher),
(@UTeacher3,      @RoleTeacher),
(@UEvangLead1,    @RoleEvangLead),
(@UEvangLead2,    @RoleEvangLead),
(@UMultimedia1,   @RoleMultimedia),
(@UMultimedia2,   @RoleMultimedia),
(@ULogistics1,    @RoleLogistics),
(@ULogistics2,    @RoleLogistics),
(@UMember1,       @RoleMember),
(@UMember2,       @RoleMember),
(@UMember3,       @RoleMember),
(@UMember4,       @RoleMember),
(@UMember5,       @RoleMember),
(@UMember6,       @RoleMember),
(@UMember7,       @RoleMember),
(@UMember8,       @RoleMember);

-- ============================================================================
-- 3. FAMILLES
-- ============================================================================
DECLARE @Family1 UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000001';
DECLARE @Family2 UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000002';
DECLARE @Family3 UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000003';
DECLARE @Family4 UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000004';
DECLARE @Family5 UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000005';
DECLARE @Family6 UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000006';
DECLARE @Family7 UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000007';

INSERT INTO Families (Id, ChurchId, Name, Notes, CreatedAt, IsDeleted) VALUES
(@Family1, @ParentChurchId, N'Famille Tshilumba', N'Famille fondatrice — commune de la Gombe',            @Now, 0),
(@Family2, @ParentChurchId, N'Famille Mbuyi',     N'Famille du trésorier principal',                       @Now, 0),
(@Family3, @ParentChurchId, N'Famille Tshisekedi', N'Famille très active dans l''évangélisation',          @Now, 0),
(@Family4, @ParentChurchId, N'Famille Kabila',    N'Famille du responsable du département louange',        @Now, 0),
(@Family5, @ParentChurchId, N'Famille Mwamba',    N'Famille de l''administratrice principale',             @Now, 0),
(@Family6, @ChildChurch1Id, N'Famille Ilunga',    N'Famille de l''antenne de Lubumbashi',                  @Now, 0),
(@Family7, @ChildChurch2Id, N'Famille Bilonda',   N'Famille de l''antenne de Goma',                        @Now, 0);

-- ============================================================================
-- 4. MEMBRES (≈ 30 membres avec accents français)
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
DECLARE @Mbr16 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000010';
DECLARE @Mbr17 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000011';
DECLARE @Mbr18 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000012';
DECLARE @Mbr19 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000013';
DECLARE @Mbr20 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000014';
DECLARE @Mbr21 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000015';
DECLARE @Mbr22 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000016';
DECLARE @Mbr23 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000017';
DECLARE @Mbr24 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000018';
DECLARE @Mbr25 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-000000000019';
DECLARE @Mbr26 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-00000000001A';
DECLARE @Mbr27 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-00000000001B';
DECLARE @Mbr28 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-00000000001C';
DECLARE @Mbr29 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-00000000001D';
DECLARE @Mbr30 UNIQUEIDENTIFIER = 'D0000001-0000-0000-0000-00000000001E';

INSERT INTO Members (Id, ChurchId, FirstName, MiddleName, LastName, DateOfBirth, Gender, MaritalStatus, Phone, Email, [Address], City, Country, MembershipNumber, [Status], JoinDate, BaptismDate, Occupation, QrCodeValue, FamilyId, FamilyRole, AppUserId, CreatedAt, IsDeleted)
VALUES
(@Mbr01, @ParentChurchId, N'Patrice',     N'Jean',       N'Tshilumba',    '1975-03-15', 1, 2, '+243 811 100 001', 'central1@eeg-grace.cd',     N'122 Avenue de la Libération, Gombe', N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0001', 1, '2008-01-15', '2008-06-20', N'Pasteur principal',      'QR-EEG-0001', @Family1, 1, @UCentralAdmin1, @Now, 0),
(@Mbr02, @ParentChurchId, N'Bénédicte',   N'Marie',      N'Kabongo',      '1978-11-02', 2, 2, '+243 811 100 002', 'central2@eeg-grace.cd',     N'33 Avenue Kasa-Vubu, Kintambo',     N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0002', 1, '2009-02-10', '2009-07-05', N'Co-pasteure',            'QR-EEG-0002', NULL,     NULL, @UCentralAdmin2, @Now, 0),
(@Mbr03, @ParentChurchId, N'Félicité',    N'Rose',       N'Mwamba',       '1982-07-22', 2, 2, '+243 811 100 003', 'admin.kin@eeg-grace.cd',    N'56 Avenue du Commerce, Lingwala',   N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0003', 1, '2010-03-01', '2010-09-15', N'Administratrice',        'QR-EEG-0003', @Family5, 2, @UChurchAdmin1,  @Now, 0),
(@Mbr04, @ChildChurch1Id, N'André',       N'Simon',      N'Ilunga',       '1980-08-12', 1, 2, '+243 822 100 004', 'admin.lshi@eeg-grace.cd',   N'45 Avenue Mobutu, Lubumbashi',      N'Lubumbashi', N'République Démocratique du Congo', 'EEG-0004', 1, '2011-01-15', '2011-07-20', N'Directeur d''antenne',   'QR-EEG-0004', @Family6, 1, @UChurchAdmin2,  @Now, 0),
(@Mbr05, @ParentChurchId, N'Célestin',    N'Paul',       N'Mukendi',      '1990-11-05', 1, 1, '+243 811 100 005', 'it1@eeg-grace.cd',          N'78 Avenue des Poids Lourds, Limete', N'Kinshasa',  N'République Démocratique du Congo', 'EEG-0005', 1, '2015-06-10', '2016-01-08', N'Ingénieur informatique', 'QR-EEG-0005', NULL,     NULL, @UITManager1,    @Now, 0),
(@Mbr06, @ParentChurchId, N'Jérémie',     N'Michel',     N'Kasongo',      '1988-05-18', 1, 1, '+243 811 100 006', 'it2@eeg-grace.cd',          N'12 Avenue Kabasele, Bandalungwa',   N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0006', 1, '2016-09-01', '2017-03-10', N'Technicien réseau',      'QR-EEG-0006', NULL,     NULL, @UITManager2,    @Now, 0),
(@Mbr07, @ParentChurchId, N'Béatrice',    N'Aimée',      N'Tshibanda',    '1985-04-18', 2, 2, '+243 811 100 007', 'secretariat1@eeg-grace.cd', N'23 Avenue du 30 Juin, Gombe',       N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0007', 1, '2012-09-01', '2013-03-10', N'Secrétaire juridique',   'QR-EEG-0007', NULL,     NULL, @USecretary1,    @Now, 0),
(@Mbr08, @ChildChurch1Id, N'Thérèse',     N'Yvonne',     N'Kalonji',      '1987-02-14', 2, 2, '+243 822 100 008', 'secretariat2@eeg-grace.cd', N'9 Avenue Kasaï, Lubumbashi',        N'Lubumbashi', N'République Démocratique du Congo', 'EEG-0008', 1, '2013-04-01', '2013-10-22', N'Secrétaire comptable',   'QR-EEG-0008', NULL,     NULL, @USecretary2,    @Now, 0),
(@Mbr09, @ParentChurchId, N'Étienne',     N'Joseph',     N'Mbuyi',        '1973-01-30', 1, 2, '+243 811 100 009', 'tresorier1@eeg-grace.cd',   N'67 Avenue Kabambare, Barumbu',      N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0009', 1, '2007-05-20', '2008-01-12', N'Expert-comptable',       'QR-EEG-0009', @Family2, 1, @UTreasurer1,    @Now, 0),
(@Mbr10, @ChildChurch2Id, N'Clémentine',  N'Flore',      N'Ngoy',         '1989-09-12', 2, 1, '+243 853 100 010', 'tresorier2@eeg-grace.cd',   N'15 Avenue du Lac, Les Volcans',     N'Goma',       N'République Démocratique du Congo', 'EEG-0010', 1, '2017-02-15', '2017-08-20', N'Comptable agréée',       'QR-EEG-0010', NULL,     NULL, @UTreasurer2,    @Now, 0),
(@Mbr11, @ParentChurchId, N'François',    N'Étienne',    N'Kabila',       '1980-08-12', 1, 2, '+243 811 100 011', 'dept.head1@eeg-grace.cd',   N'88 Avenue Patrice Lumumba, Lemba',  N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0011', 1, '2011-01-15', '2011-07-20', N'Directeur commercial',   'QR-EEG-0011', @Family4, 1, @UDeptHead1,     @Now, 0),
(@Mbr12, @ParentChurchId, N'Gérard',      N'Albert',     N'Mulumba',      '1978-06-23', 1, 2, '+243 811 100 012', 'dept.head2@eeg-grace.cd',   N'34 Avenue Tshatshi, Ngaliema',      N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0012', 1, '2010-02-08', '2010-08-15', N'Entrepreneur',           'QR-EEG-0012', NULL,     NULL, @UDeptHead2,     @Now, 0),
(@Mbr13, @ParentChurchId, N'Marie-Josée', N'Henriette',  N'Lumbala',      '1983-12-05', 2, 2, '+243 811 100 013', 'dept.head3@eeg-grace.cd',   N'51 Avenue Flambeau, Kasa-Vubu',     N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0013', 1, '2012-03-18', '2012-09-30', N'Responsable RH',         'QR-EEG-0013', NULL,     NULL, @UDeptHead3,     @Now, 0),
(@Mbr14, @ParentChurchId, N'Géraldine',   N'Flore',      N'Tshiamala',    '1988-12-03', 2, 1, '+243 811 100 014', 'dept.tres1@eeg-grace.cd',   N'19 Avenue des Jardins, Bandalungwa',N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0014', 1, '2014-04-01', '2015-01-18', N'Analyste financière',    'QR-EEG-0014', NULL,     NULL, @UDeptTreas1,    @Now, 0),
(@Mbr15, @ParentChurchId, N'Cécile',      N'Berthe',     N'Kayembe',      '1991-03-27', 2, 1, '+243 811 100 015', 'dept.tres2@eeg-grace.cd',   N'44 Avenue Kitega, Matete',          N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0015', 1, '2016-05-10', '2017-02-18', N'Auditrice interne',      'QR-EEG-0015', NULL,     NULL, @UDeptTreas2,    @Now, 0),
(@Mbr16, @ParentChurchId, N'Théophile',   N'René',       N'Banza',        '1970-06-25', 1, 2, '+243 811 100 016', 'enseignant1@eeg-grace.cd',  N'29 Avenue de l''Université, Lemba', N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0016', 1, '2006-02-10', '2006-08-15', N'Professeur de lycée',    'QR-EEG-0016', NULL,     NULL, @UTeacher1,      @Now, 0),
(@Mbr17, @ParentChurchId, N'Émilie',      N'Mireille',   N'Mukoko',       '1986-10-14', 2, 2, '+243 811 100 017', 'enseignant2@eeg-grace.cd',  N'61 Avenue Masiala, Ngiri-Ngiri',    N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0017', 1, '2013-08-20', '2014-02-11', N'Institutrice',           'QR-EEG-0017', NULL,     NULL, @UTeacher2,      @Now, 0),
(@Mbr18, @ChildChurch1Id, N'Noël',        N'Gervais',    N'Kasumba',      '1984-12-25', 1, 2, '+243 822 100 018', 'enseignant3@eeg-grace.cd',  N'7 Avenue Sendwe, Lubumbashi',       N'Lubumbashi', N'République Démocratique du Congo', 'EEG-0018', 1, '2015-01-08', '2015-07-11', N'Enseignant de théologie','QR-EEG-0018', NULL,     NULL, @UTeacher3,      @Now, 0),
(@Mbr19, @ParentChurchId, N'Moïse',       N'Samuel',     N'Tshisekedi',   '1981-09-08', 1, 2, '+243 811 100 019', 'evangelisme1@eeg-grace.cd', N'100 Avenue Kamanyola, Masina',      N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0019', 1, '2009-11-01', '2010-04-22', N'Évangéliste',            'QR-EEG-0019', @Family3, 1, @UEvangLead1,    @Now, 0),
(@Mbr20, @ChildChurch2Id, N'Daniel',      N'Pierre',     N'Kankonde',     '1987-07-19', 1, 2, '+243 853 100 020', 'evangelisme2@eeg-grace.cd', N'22 Avenue Mikeno, Goma',            N'Goma',       N'République Démocratique du Congo', 'EEG-0020', 1, '2014-06-14', '2014-12-01', N'Missionnaire',           'QR-EEG-0020', NULL,     NULL, @UEvangLead2,    @Now, 0),
(@Mbr21, @ParentChurchId, N'Raphaël',     N'Dieudonné',  N'Bemba',        '1992-12-25', 1, 1, '+243 811 100 021', 'multimedia1@eeg-grace.cd',  N'14 Avenue Kalambay, Kalamu',        N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0021', 1, '2017-01-08', '2017-06-11', N'Vidéaste',               'QR-EEG-0021', NULL,     NULL, @UMultimedia1,   @Now, 0),
(@Mbr22, @ParentChurchId, N'Stéphane',    N'Didier',     N'Lukusa',       '1995-04-02', 1, 1, '+243 811 100 022', 'multimedia2@eeg-grace.cd',  N'5 Avenue Kapela, Kasa-Vubu',        N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0022', 1, '2019-09-12', '2020-03-15', N'Ingénieur du son',       'QR-EEG-0022', NULL,     NULL, @UMultimedia2,   @Now, 0),
(@Mbr23, @ParentChurchId, N'Éric',        N'Pascal',     N'Lumumba',      '1987-05-14', 1, 2, '+243 811 100 023', 'logistique1@eeg-grace.cd',  N'26 Avenue Livulu, Lemba',           N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0023', 1, '2013-07-01', '2014-02-16', N'Logisticien',            'QR-EEG-0023', NULL,     NULL, @ULogistics1,    @Now, 0),
(@Mbr24, @ChildChurch1Id, N'Joseph',      N'Antoine',    N'Kasa-Vubu',    '1982-03-08', 1, 2, '+243 822 100 024', 'logistique2@eeg-grace.cd',  N'37 Avenue Kilela, Lubumbashi',      N'Lubumbashi', N'République Démocratique du Congo', 'EEG-0024', 1, '2012-04-15', '2012-10-20', N'Gestionnaire de stock',  'QR-EEG-0024', NULL,     NULL, @ULogistics2,    @Now, 0),
(@Mbr25, @ParentChurchId, N'Joséphine',   N'Grâce',      N'Mbuyi',        '1995-02-28', 2, 1, '+243 811 100 025', 'membre1@eeg-grace.cd',      N'67 Avenue Kabambare, Barumbu',      N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0025', 1, '2019-03-10', '2019-09-22', N'Infirmière',             'QR-EEG-0025', @Family2, 3, @UMember1,       @Now, 0),
(@Mbr26, @ParentChurchId, N'Emmanuel',    N'Thierry',    N'Tshibola',     '1998-10-17', 1, 1, '+243 811 100 026', 'membre2@eeg-grace.cd',      N'82 Avenue de la Paix, Ndjili',      N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0026', 1, '2020-06-01', '2021-01-10', N'Étudiant en médecine',   'QR-EEG-0026', NULL,     NULL, @UMember2,       @Now, 0),
(@Mbr27, @ParentChurchId, N'Élisabeth',   N'Colette',    N'Ntumba',       '1991-08-09', 2, 2, '+243 811 100 027', 'membre3@eeg-grace.cd',      N'13 Avenue Kimbangu, Kintambo',      N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0027', 1, '2018-01-15', '2018-07-08', N'Enseignante',            'QR-EEG-0027', NULL,     NULL, @UMember3,       @Now, 0),
(@Mbr28, @ParentChurchId, N'Bernadette',  N'Solange',    N'Mukeba',       '1994-11-23', 2, 1, '+243 811 100 028', 'membre4@eeg-grace.cd',      N'47 Avenue Wagenia, Kimbanseke',     N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0028', 1, '2020-02-10', '2020-08-30', N'Couturière',             'QR-EEG-0028', NULL,     NULL, @UMember4,       @Now, 0),
(@Mbr29, @ParentChurchId, N'Frédéric',    N'Honoré',     N'Mbombo',       '1993-06-18', 1, 1, '+243 811 100 029', 'membre5@eeg-grace.cd',      N'98 Avenue Tshela, Lemba',           N'Kinshasa',   N'République Démocratique du Congo', 'EEG-0029', 1, '2020-11-05', '2021-05-14', N'Chauffeur',              'QR-EEG-0029', NULL,     NULL, @UMember5,       @Now, 0),
(@Mbr30, @ChildChurch2Id, N'Véronique',   N'Espérance',  N'Bilonda',      '1989-04-05', 2, 2, '+243 853 100 031', 'membre7@eeg-grace.cd',      N'18 Avenue du Lac, Les Volcans',     N'Goma',       N'République Démocratique du Congo', 'EEG-0030', 1, '2018-03-20', '2018-09-09', N'Commerçante',            'QR-EEG-0030', @Family7, 1, @UMember7,       @Now, 0);

-- ============================================================================
-- 5. VISITEURS
-- ============================================================================
INSERT INTO Visitors (Id, ChurchId, FirstName, LastName, Phone, Email, Gender, FirstVisitDate, HowHeardAboutUs, Notes, [Status], CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, N'Gaëtan',   N'Mbaya',    '+243 820 111 222', 'gaetan.m@email.cd',    1, '2026-03-15', N'Invité par un ami',              N'Très intéressé par le groupe des jeunes',            1, @Now, 0),
(NEWID(), @ParentChurchId, N'Félicie',  N'Ngalula',  '+243 820 333 444', 'felicie.n@email.cd',   2, '2026-03-22', N'Publicité sur les réseaux sociaux',N'Première visite, a demandé des informations',         2, @Now, 0),
(NEWID(), @ParentChurchId, N'Jérémie',  N'Tshiungu', '+243 820 555 666', NULL,                   1, '2026-04-01', N'Passait devant le temple',       N'Recherche une communauté chrétienne chaleureuse',     1, @Now, 0),
(NEWID(), @ParentChurchId, N'Pélagie',  N'Mfumu',    '+243 820 777 888', 'pelagie.m@email.cd',   2, '2026-04-05', N'Recommandée par une voisine',    N'Vient de déménager à Kinshasa — cherche une église',  3, @Now, 0),
(NEWID(), @ChildChurch1Id, N'Blaise',   N'Kibwe',    '+243 820 999 000', 'blaise.k@email.cd',    1, '2026-04-08', N'Entendu à la radio',             N'Employé d''une société minière — disponible le dimanche', 2, @Now, 0),
(NEWID(), @ChildChurch2Id, N'Solange',  N'Mapendo',  '+243 820 111 333', NULL,                   2, '2026-04-12', N'Campagne d''évangélisation',     N'A donné son cœur à Christ lors de la campagne',       1, @Now, 0);

-- ============================================================================
-- 6. FONDS
-- ============================================================================
DECLARE @FundGeneral   UNIQUEIDENTIFIER = 'E0000001-0000-0000-0000-000000000001';
DECLARE @FundBuilding  UNIQUEIDENTIFIER = 'E0000001-0000-0000-0000-000000000002';
DECLARE @FundMissions  UNIQUEIDENTIFIER = 'E0000001-0000-0000-0000-000000000003';
DECLARE @FundSocial    UNIQUEIDENTIFIER = 'E0000001-0000-0000-0000-000000000004';

INSERT INTO Funds (Id, ChurchId, Name, [Description], IsDefault, IsActive, CreatedAt, IsDeleted) VALUES
(@FundGeneral,  @ParentChurchId, N'Fonds général',          N'Dîmes et offrandes courantes du dimanche',                   1, 1, @Now, 0),
(@FundBuilding, @ParentChurchId, N'Fonds de construction',  N'Construction du nouveau temple à Kinshasa',                   0, 1, @Now, 0),
(@FundMissions, @ParentChurchId, N'Fonds des missions',     N'Soutien aux missions d''évangélisation à l''intérieur du pays', 0, 1, @Now, 0),
(@FundSocial,   @ParentChurchId, N'Fonds social',           N'Aide aux veuves, orphelins et personnes démunies',            0, 1, @Now, 0);

-- ============================================================================
-- 7. CAMPAGNES DE CONTRIBUTIONS
-- ============================================================================
DECLARE @Campaign1 UNIQUEIDENTIFIER = 'E0000002-0000-0000-0000-000000000001';
DECLARE @Campaign2 UNIQUEIDENTIFIER = 'E0000002-0000-0000-0000-000000000002';

INSERT INTO ContributionCampaigns (Id, ChurchId, Name, [Description], TargetAmount, Currency, StartDate, EndDate, [Status], FundId, CreatedAt, IsDeleted) VALUES
(@Campaign1, @ParentChurchId, N'Bâtissons ensemble 2026',  N'Campagne pour la construction du nouveau bâtiment du temple de Kinshasa',   150000000.00, 'CDF', '2026-01-01', '2026-12-31', 2, @FundBuilding, @Now, 0),
(@Campaign2, @ParentChurchId, N'Noël des enfants 2026',     N'Collecte pour la fête de Noël des orphelins — cadeaux et repas chaleureux',   8000000.00, 'CDF', '2026-10-01', '2026-12-25', 2, @FundSocial,   @Now, 0);

-- ============================================================================
-- 8. CONTRIBUTIONS
-- ============================================================================
INSERT INTO Contributions (Id, ChurchId, ReferenceNumber, Amount, Currency, ContributionDate, [Type], [Status], Notes, MemberId, FundId, CampaignId, IsRecurring, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, 'CTR-2026-001',  450000.00, 'CDF', '2026-01-07', 1, 2, N'Dîme de janvier',                     @Mbr01, @FundGeneral,  NULL,       0, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-002',  225000.00, 'CDF', '2026-01-07', 1, 2, N'Offrande du dimanche',                @Mbr09, @FundGeneral,  NULL,       0, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-003', 1500000.00, 'CDF', '2026-01-15', 3, 2, N'Don pour la construction',             @Mbr11, @FundBuilding, @Campaign1, 0, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-004',  150000.00, 'CDF', '2026-02-04', 1, 2, N'Dîme de février',                     @Mbr25, @FundGeneral,  NULL,       0, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-005',  600000.00, 'CDF', '2026-02-14', 4, 2, N'Contribution via M-Pesa',              @Mbr19, @FundMissions, NULL,       0, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-006',  300000.00, 'CDF', '2026-03-02', 1, 2, N'Dîme de mars',                         @Mbr03, @FundGeneral,  NULL,       1, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-007', 3000000.00, 'CDF', '2026-03-20', 3, 2, N'Don spécial construction',             @Mbr01, @FundBuilding, @Campaign1, 0, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-008',  500000.00, 'CDF', '2026-03-28', 2, 2, N'Offrande spéciale — culte d''action de grâce', @Mbr13, @FundGeneral, NULL, 0, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-009',  180000.00, 'CDF', '2026-04-05', 1, 2, N'Dîme d''avril',                        @Mbr27, @FundGeneral,  NULL,       0, @Now, 0),
(NEWID(), @ParentChurchId, 'CTR-2026-010',  750000.00, 'CDF', '2026-04-10', 3, 2, N'Don pour missions à Kisangani',        @Mbr12, @FundMissions, NULL,       0, @Now, 0),
(NEWID(), @ChildChurch1Id, 'CTR-2026-011',  220000.00, 'CDF', '2026-03-15', 1, 2, N'Dîme antenne Lubumbashi',              @Mbr04, @FundGeneral,  NULL,       0, @Now, 0),
(NEWID(), @ChildChurch2Id, 'CTR-2026-012',  140000.00, 'CDF', '2026-03-18', 1, 2, N'Dîme antenne Goma',                    @Mbr30, @FundGeneral,  NULL,       0, @Now, 0);

-- ============================================================================
-- 9. PROMESSES (Pledges)
-- ============================================================================
INSERT INTO Pledges (Id, ChurchId, MemberId, FundId, CampaignId, PledgedAmount, PaidAmount, Currency, StartDate, EndDate, [Status], Notes, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Mbr01, @FundBuilding, @Campaign1, 15000000.00, 4500000.00, 'CDF', '2026-01-01', '2026-12-31', 1, N'Engagement annuel pour la construction',   @Now, 0),
(NEWID(), @ParentChurchId, @Mbr09, @FundBuilding, @Campaign1,  6000000.00, 1500000.00, 'CDF', '2026-01-01', '2026-12-31', 1, N'Promesse de contribution trimestrielle',  @Now, 0),
(NEWID(), @ParentChurchId, @Mbr11, @FundBuilding, @Campaign1,  9000000.00, 1500000.00, 'CDF', '2026-01-01', '2026-12-31', 1, N'Engagement familial',                     @Now, 0),
(NEWID(), @ParentChurchId, @Mbr13, @FundSocial,   @Campaign2,  2000000.00,  500000.00, 'CDF', '2026-10-01', '2026-12-25', 1, N'Promesse pour la fête de Noël',          @Now, 0);

-- ============================================================================
-- 10. COMPTES BANCAIRES
-- ============================================================================
DECLARE @BankAcct1 UNIQUEIDENTIFIER = 'E0000003-0000-0000-0000-000000000001';
DECLARE @BankAcct2 UNIQUEIDENTIFIER = 'E0000003-0000-0000-0000-000000000002';
DECLARE @BankAcct3 UNIQUEIDENTIFIER = 'E0000003-0000-0000-0000-000000000003';

INSERT INTO BankAccounts (Id, ChurchId, AccountName, AccountNumber, BankName, BranchName, AccountType, Currency, CurrentBalance, IsActive, IsDefault, CreatedAt, IsDeleted) VALUES
(@BankAcct1, @ParentChurchId, N'Compte courant principal',  '00010-01001-00123456-78',  N'Rawbank',               N'Agence Gombe',      3, 'CDF', 37500000.00, 1, 1, @Now, 0),
(@BankAcct2, @ParentChurchId, N'Compte épargne projet',      '00020-02001-00987654-32',  N'Trust Merchant Bank',   N'Agence Gombe',      2, 'CDF', 25000000.00, 1, 0, @Now, 0),
(@BankAcct3, @ParentChurchId, N'Compte devises USD',         '00030-03001-00456789-12',  N'Equity BCDC',           N'Agence Kinshasa',   3, 'USD',    18500.00, 1, 0, @Now, 0);

-- ============================================================================
-- 11. TRANSACTIONS BANCAIRES
-- ============================================================================
INSERT INTO AccountTransactions (Id, ChurchId, BankAccountId, [Type], Amount, Currency, TransactionDate, [Description], RunningBalance, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @BankAcct1, 1, 6225000.00, 'CDF', '2026-01-31', N'Versement dîmes et offrandes — janvier 2026',   37500000.00, @Now, 0),
(NEWID(), @ParentChurchId, @BankAcct2, 1, 4500000.00, 'CDF', '2026-02-28', N'Transfert vers compte épargne projet',           25000000.00, @Now, 0),
(NEWID(), @ParentChurchId, @BankAcct1, 2, 1050000.00, 'CDF', '2026-03-05', N'Paiement facture d''électricité — mars 2026',   36450000.00, @Now, 0),
(NEWID(), @ParentChurchId, @BankAcct3, 1, 2500.00,    'USD', '2026-03-15', N'Don en devises reçu d''un missionnaire',          18500.00,    @Now, 0),
(NEWID(), @ParentChurchId, @BankAcct1, 2, 850000.00,  'CDF', '2026-04-10', N'Paiement location du centre de conférences',     35600000.00, @Now, 0);

-- ============================================================================
-- 12. CATÉGORIES DE DÉPENSES
-- ============================================================================
DECLARE @ExpCatUtilities UNIQUEIDENTIFIER = 'E0000004-0000-0000-0000-000000000001';
DECLARE @ExpCatSalaries  UNIQUEIDENTIFIER = 'E0000004-0000-0000-0000-000000000002';
DECLARE @ExpCatEvents    UNIQUEIDENTIFIER = 'E0000004-0000-0000-0000-000000000003';
DECLARE @ExpCatSupplies  UNIQUEIDENTIFIER = 'E0000004-0000-0000-0000-000000000004';
DECLARE @ExpCatTransport UNIQUEIDENTIFIER = 'E0000004-0000-0000-0000-000000000005';

INSERT INTO ExpenseCategories (Id, ChurchId, Name, [Description], Color, CreatedAt, IsDeleted) VALUES
(@ExpCatUtilities, @ParentChurchId, N'Services publics',      N'Électricité (SNEL), eau (REGIDESO), internet', '#FF5722', @Now, 0),
(@ExpCatSalaries,  @ParentChurchId, N'Salaires et primes',    N'Rémunération du personnel et indemnités',       '#2196F3', @Now, 0),
(@ExpCatEvents,    @ParentChurchId, N'Événements',            N'Organisation des événements spéciaux',           '#4CAF50', @Now, 0),
(@ExpCatSupplies,  @ParentChurchId, N'Fournitures de bureau', N'Papeterie, encre, matériel divers',             '#9C27B0', @Now, 0),
(@ExpCatTransport, @ParentChurchId, N'Transport et carburant',N'Carburant véhicules et déplacements',           '#795548', @Now, 0);

-- ============================================================================
-- 13. DÉPENSES
-- ============================================================================
INSERT INTO Expenses (Id, ChurchId, Title, [Description], Amount, Currency, ExpenseDate, [Status], PaymentMethod, VendorName, CategoryId, BankAccountId, SubmittedByMemberId, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, N'Facture électricité — mars 2026', N'SNEL — consommation du temple principal',     1050000.00, 'CDF', '2026-03-05', 5, 3, N'SNEL',                        @ExpCatUtilities, @BankAcct1, @Mbr09, @Now, 0),
(NEWID(), @ParentChurchId, N'Facture eau — mars 2026',          N'REGIDESO — consommation mensuelle',            350000.00,  'CDF', '2026-03-08', 5, 3, N'REGIDESO',                    @ExpCatUtilities, @BankAcct1, @Mbr09, @Now, 0),
(NEWID(), @ParentChurchId, N'Salaire gardien mars',             N'Rémunération mensuelle du gardien',            400000.00,  'CDF', '2026-03-28', 5, 1, NULL,                           @ExpCatSalaries,  @BankAcct1, @Mbr09, @Now, 0),
(NEWID(), @ParentChurchId, N'Salaire secrétaire mars',          N'Rémunération mensuelle de la secrétaire',      600000.00,  'CDF', '2026-03-28', 5, 1, NULL,                           @ExpCatSalaries,  @BankAcct1, @Mbr09, @Now, 0),
(NEWID(), @ParentChurchId, N'Fournitures culte de Pâques',     N'Décoration, programmes imprimés',               285000.00,  'CDF', '2026-04-01', 3, 1, N'Imprimerie Tshisekedi',       @ExpCatEvents,    NULL,       @Mbr07, @Now, 0),
(NEWID(), @ParentChurchId, N'Abonnement internet fibre',         N'Vodacom Business — forfait mensuel',           245000.00,  'CDF', '2026-04-05', 2, 3, N'Vodacom RDC',                 @ExpCatUtilities, NULL,       @Mbr05, @Now, 0),
(NEWID(), @ParentChurchId, N'Carburant pour bus conférence',    N'Déplacement vers conférence de Pâques',        220000.00,  'CDF', '2026-04-18', 5, 1, N'Station Engen Gombe',         @ExpCatTransport, @BankAcct1, @Mbr23, @Now, 0),
(NEWID(), @ParentChurchId, N'Location centre de conférences',    N'Conférence de Pâques 2026 — 3 jours',          850000.00,  'CDF', '2026-04-10', 5, 3, N'Centre Pullman Kinshasa',    @ExpCatEvents,    @BankAcct1, @Mbr07, @Now, 0);

-- ============================================================================
-- 14. BUDGETS ET LIGNES BUDGÉTAIRES
-- ============================================================================
DECLARE @Budget2026 UNIQUEIDENTIFIER = 'E0000005-0000-0000-0000-000000000001';

INSERT INTO Budgets (Id, ChurchId, Name, [Year], StartDate, EndDate, Currency, TotalAmount, [Status], Notes, CreatedAt, IsDeleted) VALUES
(@Budget2026, @ParentChurchId, N'Budget annuel 2026', 2026, '2026-01-01', '2026-12-31', 'CDF', 45000000.00, 2, N'Budget approuvé en assemblée générale du 15 décembre 2025', @Now, 0);

INSERT INTO BudgetLines (Id, ChurchId, BudgetId, CategoryId, Name, AllocatedAmount, SpentAmount, Notes, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Budget2026, @ExpCatUtilities, N'Services publics annuels',    12600000.00, 1645000.00, N'Électricité + eau + internet',  @Now, 0),
(NEWID(), @ParentChurchId, @Budget2026, @ExpCatSalaries,  N'Masse salariale',              18000000.00, 1000000.00, N'Gardien + secrétaire + pasteur', @Now, 0),
(NEWID(), @ParentChurchId, @Budget2026, @ExpCatEvents,    N'Événements et célébrations',    8000000.00, 1135000.00, N'Pâques, Noël, conférences',     @Now, 0),
(NEWID(), @ParentChurchId, @Budget2026, @ExpCatSupplies,  N'Fournitures diverses',          3400000.00,       0,    N'Bureau et entretien',            @Now, 0),
(NEWID(), @ParentChurchId, @Budget2026, @ExpCatTransport, N'Transport et carburant',        3000000.00,  220000.00, N'Véhicules et missions',          @Now, 0);

-- ============================================================================
-- 15. ÉVÉNEMENTS D'ÉGLISE
-- ============================================================================
DECLARE @Event1 UNIQUEIDENTIFIER = 'F0000001-0000-0000-0000-000000000001';
DECLARE @Event2 UNIQUEIDENTIFIER = 'F0000001-0000-0000-0000-000000000002';
DECLARE @Event3 UNIQUEIDENTIFIER = 'F0000001-0000-0000-0000-000000000003';
DECLARE @Event4 UNIQUEIDENTIFIER = 'F0000001-0000-0000-0000-000000000004';
DECLARE @Event5 UNIQUEIDENTIFIER = 'F0000001-0000-0000-0000-000000000005';

INSERT INTO ChurchEvents (Id, ChurchId, Title, [Description], [Type], [Status], StartDateTime, EndDateTime, [Location], RequiresRegistration, MaxAttendees, QrCodeValue, IsRecurring, RecurrenceFrequency, CreatedAt, IsDeleted) VALUES
(@Event1, @ParentChurchId, N'Culte dominical',                  N'Célébration du dimanche avec louange et prédication',          0, 1, '2026-04-19 09:00:00', '2026-04-19 12:00:00', N'Temple principal — Gombe, Kinshasa',    0, 800, 'QR-EVT-001', 1, 1, @Now, 0),
(@Event2, @ParentChurchId, N'Conférence de Pâques 2026',        N'Trois jours de retraite spirituelle avec orateurs invités',    1, 1, '2026-04-17 08:00:00', '2026-04-19 17:00:00', N'Centre Pullman — Kinshasa',              1, 400, 'QR-EVT-002', 0, NULL, @Now, 0),
(@Event3, @ParentChurchId, N'Soirée de louange et prière',      N'Veillée de prière avec l''équipe de louange',                  8, 3, '2026-03-28 19:00:00', '2026-03-29 05:00:00', N'Temple principal — Gombe, Kinshasa',    0, NULL, 'QR-EVT-003', 0, NULL, @Now, 0),
(@Event4, @ParentChurchId, N'Mariage — Éric et Bernadette',      N'Cérémonie nuptiale et bénédiction',                            7, 1, '2026-05-30 14:00:00', '2026-05-30 18:00:00', N'Temple principal — Gombe, Kinshasa',    1, 250, 'QR-EVT-004', 0, NULL, @Now, 0),
(@Event5, @ChildChurch1Id, N'Culte dominical — Lubumbashi',     N'Célébration du dimanche à l''antenne',                         0, 1, '2026-04-19 09:30:00', '2026-04-19 12:30:00', N'Temple de l''antenne — Lubumbashi',      0, 300, 'QR-EVT-005', 1, 1, @Now, 0);

-- ============================================================================
-- 16. INSCRIPTIONS AUX ÉVÉNEMENTS
-- ============================================================================
INSERT INTO EventRegistrations (Id, ChurchId, EventId, MemberId, [Status], RegistrationCode, IsPaid, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Event2, @Mbr01, 1, 'REG-PAQUES-001', 1, @Now, 0),
(NEWID(), @ParentChurchId, @Event2, @Mbr07, 1, 'REG-PAQUES-002', 1, @Now, 0),
(NEWID(), @ParentChurchId, @Event2, @Mbr19, 1, 'REG-PAQUES-003', 1, @Now, 0),
(NEWID(), @ParentChurchId, @Event2, @Mbr25, 0, 'REG-PAQUES-004', 0, @Now, 0),
(NEWID(), @ParentChurchId, @Event2, @Mbr26, 1, 'REG-PAQUES-005', 1, @Now, 0),
(NEWID(), @ParentChurchId, @Event2, @Mbr11, 1, 'REG-PAQUES-006', 1, @Now, 0),
(NEWID(), @ParentChurchId, @Event4, @Mbr23, 1, 'REG-MAR-001',    0, @Now, 0),
(NEWID(), @ParentChurchId, @Event4, @Mbr28, 1, 'REG-MAR-002',    0, @Now, 0);

-- ============================================================================
-- 17. PRÉSENCES AUX ÉVÉNEMENTS
-- ============================================================================
INSERT INTO EventAttendances (Id, ChurchId, EventId, MemberId, AttendanceDate, [Status], RecordedByQr, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Event3, @Mbr01, '2026-03-28', 0, 0, @Now, 0),
(NEWID(), @ParentChurchId, @Event3, @Mbr07, '2026-03-28', 0, 0, @Now, 0),
(NEWID(), @ParentChurchId, @Event3, @Mbr09, '2026-03-28', 0, 1, @Now, 0),
(NEWID(), @ParentChurchId, @Event3, @Mbr16, '2026-03-28', 0, 0, @Now, 0),
(NEWID(), @ParentChurchId, @Event3, @Mbr19, '2026-03-28', 2, 0, @Now, 0),
(NEWID(), @ParentChurchId, @Event3, @Mbr25, '2026-03-28', 1, 0, @Now, 0),
(NEWID(), @ParentChurchId, @Event3, @Mbr27, '2026-03-28', 0, 1, @Now, 0);

-- ============================================================================
-- 18. ÉCOLE DU DIMANCHE
-- ============================================================================
DECLARE @SSClass1 UNIQUEIDENTIFIER = 'F0000002-0000-0000-0000-000000000001';
DECLARE @SSClass2 UNIQUEIDENTIFIER = 'F0000002-0000-0000-0000-000000000002';
DECLARE @SSClass3 UNIQUEIDENTIFIER = 'F0000002-0000-0000-0000-000000000003';

INSERT INTO SundaySchoolClasses (Id, ChurchId, Name, [Description], [Level], IsActive, TeacherId, MinAge, MaxAge, MaxCapacity, CreatedAt, IsDeleted) VALUES
(@SSClass1, @ParentChurchId, N'Les Petits Bergers',       N'Classe pour enfants de 3 à 5 ans',       0, 1, @Mbr17, 3,  5,  25, @Now, 0),
(@SSClass2, @ParentChurchId, N'Les Étoiles du Matin',     N'Classe pour enfants de 6 à 10 ans',      2, 1, @Mbr16, 6,  10, 35, @Now, 0),
(@SSClass3, @ParentChurchId, N'Les Flambeaux de la Foi',  N'Classe pour adolescents de 11 à 17 ans', 4, 1, @Mbr16, 11, 17, 30, @Now, 0);

DECLARE @SSLesson1 UNIQUEIDENTIFIER = 'F0000002-1000-0000-0000-000000000001';
DECLARE @SSLesson2 UNIQUEIDENTIFIER = 'F0000002-1000-0000-0000-000000000002';

INSERT INTO SundaySchoolLessons (Id, ChurchId, ClassId, Title, [Description], LessonDate, DurationMinutes, CreatedAt, IsDeleted) VALUES
(@SSLesson1, @ParentChurchId, @SSClass2, N'L''histoire de Noé et de l''arche',   N'Genèse 6-9 — La fidélité de Dieu dans l''épreuve',     '2026-04-05', 45, @Now, 0),
(@SSLesson2, @ParentChurchId, @SSClass2, N'David et Goliath',                     N'1 Samuel 17 — Le courage face à l''adversité',         '2026-04-12', 45, @Now, 0),
(NEWID(),    @ParentChurchId, @SSClass3, N'Les Béatitudes',                       N'Matthieu 5 — Les valeurs du Royaume',                  '2026-04-05', 60, @Now, 0),
(NEWID(),    @ParentChurchId, @SSClass3, N'La prière du Notre Père',              N'Matthieu 6 — Apprendre à prier',                       '2026-04-12', 60, @Now, 0),
(NEWID(),    @ParentChurchId, @SSClass1, N'Jésus aime les petits enfants',         N'Marc 10:13-16 — L''amour de Jésus pour les enfants',  '2026-04-05', 30, @Now, 0);

INSERT INTO SundaySchoolEnrollments (Id, ChurchId, ClassId, MemberId, EnrolledDate, [Status], CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @SSClass2, @Mbr25, '2026-01-15', 0, @Now, 0),
(NEWID(), @ParentChurchId, @SSClass2, @Mbr26, '2026-01-15', 0, @Now, 0),
(NEWID(), @ParentChurchId, @SSClass3, @Mbr14, '2026-02-01', 0, @Now, 0),
(NEWID(), @ParentChurchId, @SSClass3, @Mbr15, '2026-02-01', 0, @Now, 0);

INSERT INTO SundaySchoolAttendances (Id, ChurchId, LessonId, MemberId, [Status], CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @SSLesson1, @Mbr25, 0, @Now, 0),
(NEWID(), @ParentChurchId, @SSLesson1, @Mbr26, 0, @Now, 0),
(NEWID(), @ParentChurchId, @SSLesson2, @Mbr25, 0, @Now, 0),
(NEWID(), @ParentChurchId, @SSLesson2, @Mbr26, 2, @Now, 0);

-- ============================================================================
-- 19. ÉCOLE DE CROISSANCE
-- ============================================================================
DECLARE @GSCourse1 UNIQUEIDENTIFIER = 'F0000003-0000-0000-0000-000000000001';
DECLARE @GSCourse2 UNIQUEIDENTIFIER = 'F0000003-0000-0000-0000-000000000002';

INSERT INTO GrowthSchoolCourses (Id, ChurchId, Name, [Description], [Level], IsActive, InstructorId, DurationWeeks, MaxCapacity, CreatedAt, IsDeleted) VALUES
(@GSCourse1, @ParentChurchId, N'Fondements de la foi chrétienne', N'Cours de base pour les nouveaux convertis — découverte des vérités essentielles', 0, 1, @Mbr16, 12, 25, @Now, 0),
(@GSCourse2, @ParentChurchId, N'École du leadership chrétien',    N'Formation des futurs responsables et serviteurs',                                   2, 1, @Mbr16, 16, 15, @Now, 0);

DECLARE @GSSession1 UNIQUEIDENTIFIER = 'F0000003-1000-0000-0000-000000000001';
DECLARE @GSSession2 UNIQUEIDENTIFIER = 'F0000003-1000-0000-0000-000000000002';

INSERT INTO GrowthSchoolSessions (Id, ChurchId, CourseId, Title, [Description], SessionDate, DurationMinutes, CreatedAt, IsDeleted) VALUES
(@GSSession1, @ParentChurchId, @GSCourse1, N'Qui est Dieu ?',              N'Leçon 1 — Les attributs de Dieu',           '2026-04-05', 90, @Now, 0),
(@GSSession2, @ParentChurchId, @GSCourse1, N'L''autorité de la Bible',     N'Leçon 2 — Pourquoi la Bible est fiable',    '2026-04-12', 90, @Now, 0),
(NEWID(),     @ParentChurchId, @GSCourse1, N'Le salut en Jésus-Christ',    N'Leçon 3 — La grâce seule sauve',             '2026-04-19', 90, @Now, 0),
(NEWID(),     @ParentChurchId, @GSCourse2, N'Le caractère du leader',       N'Leçon 1 — Intégrité et humilité',            '2026-04-06', 120, @Now, 0);

INSERT INTO GrowthSchoolEnrollments (Id, ChurchId, CourseId, MemberId, EnrolledDate, [Status], CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @GSCourse1, @Mbr25, '2026-03-15', 0, @Now, 0),
(NEWID(), @ParentChurchId, @GSCourse1, @Mbr26, '2026-03-15', 0, @Now, 0),
(NEWID(), @ParentChurchId, @GSCourse1, @Mbr27, '2026-03-15', 0, @Now, 0),
(NEWID(), @ParentChurchId, @GSCourse2, @Mbr11, '2026-03-20', 0, @Now, 0),
(NEWID(), @ParentChurchId, @GSCourse2, @Mbr13, '2026-03-20', 0, @Now, 0);

INSERT INTO GrowthSchoolAttendances (Id, ChurchId, SessionId, MemberId, [Status], CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @GSSession1, @Mbr25, 0, @Now, 0),
(NEWID(), @ParentChurchId, @GSSession1, @Mbr26, 0, @Now, 0),
(NEWID(), @ParentChurchId, @GSSession1, @Mbr27, 0, @Now, 0),
(NEWID(), @ParentChurchId, @GSSession2, @Mbr25, 0, @Now, 0),
(NEWID(), @ParentChurchId, @GSSession2, @Mbr26, 2, @Now, 0);

-- ============================================================================
-- 20. DÉPARTEMENTS
-- ============================================================================
DECLARE @DeptWorship UNIQUEIDENTIFIER = 'F0000004-0000-0000-0000-000000000001';
DECLARE @DeptYouth   UNIQUEIDENTIFIER = 'F0000004-0000-0000-0000-000000000002';
DECLARE @DeptWomen   UNIQUEIDENTIFIER = 'F0000004-0000-0000-0000-000000000003';
DECLARE @DeptMen     UNIQUEIDENTIFIER = 'F0000004-0000-0000-0000-000000000004';
DECLARE @DeptChildren UNIQUEIDENTIFIER = 'F0000004-0000-0000-0000-000000000005';

INSERT INTO Departments (Id, ChurchId, Name, [Description], LeaderId, Color, IsActive, CreatedAt, IsDeleted) VALUES
(@DeptWorship,  @ParentChurchId, N'Département de louange',       N'Chorales, musiciens et équipe technique de sonorisation',     @Mbr11, '#1565C0', 1, @Now, 0),
(@DeptYouth,    @ParentChurchId, N'Département de la jeunesse',   N'Activités et encadrement des jeunes de 18 à 35 ans',          @Mbr12, '#FF9800', 1, @Now, 0),
(@DeptWomen,    @ParentChurchId, N'Département des femmes',       N'Groupe Déborah — fraternité et entraide féminine',             @Mbr13, '#E91E63', 1, @Now, 0),
(@DeptMen,      @ParentChurchId, N'Département des hommes',       N'Groupe Barnabé — fraternité et mentorat masculin',             @Mbr11, '#00796B', 1, @Now, 0),
(@DeptChildren, @ParentChurchId, N'Département des enfants',      N'Encadrement des enfants durant le culte',                      @Mbr17, '#8BC34A', 1, @Now, 0);

INSERT INTO DepartmentMembers (Id, ChurchId, DepartmentId, MemberId, [Role], JoinedDate, IsActive, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @DeptWorship, @Mbr11, 0, '2020-01-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWorship, @Mbr21, 4, '2020-06-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWorship, @Mbr22, 4, '2021-01-15', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWorship, @Mbr25, 4, '2021-06-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptYouth,   @Mbr12, 0, '2019-09-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptYouth,   @Mbr26, 4, '2022-03-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptYouth,   @Mbr27, 4, '2022-03-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWomen,   @Mbr13, 0, '2018-01-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWomen,   @Mbr03, 3, '2018-06-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWomen,   @Mbr07, 2, '2019-01-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWomen,   @Mbr28, 4, '2020-03-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptMen,     @Mbr11, 0, '2019-05-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptMen,     @Mbr09, 3, '2019-05-01', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptMen,     @Mbr23, 4, '2020-01-10', 1, @Now, 0),
(NEWID(), @ParentChurchId, @DeptChildren, @Mbr17, 0, '2020-01-01', 1, @Now, 0);

INSERT INTO DepartmentTransactions (Id, ChurchId, DepartmentId, [Type], Amount, [Description], [Date], RecordedByMemberId, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @DeptWorship, 0, 750000.00,  N'Cotisations membres — trimestre 1',             '2026-03-31', @Mbr14, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWorship, 1, 540000.00,  N'Achat de cordes de guitare et microphones',     '2026-03-15', @Mbr14, @Now, 0),
(NEWID(), @ParentChurchId, @DeptWomen,   0, 525000.00,  N'Cotisations mensuelles — février',               '2026-02-28', @Mbr14, @Now, 0),
(NEWID(), @ParentChurchId, @DeptYouth,   1, 280000.00,  N'Sortie des jeunes à la plage de Kinkole',        '2026-03-20', @Mbr15, @Now, 0);

-- ============================================================================
-- 21. MESSAGERIE ET NOTIFICATIONS
-- ============================================================================
DECLARE @MsgCampaign1 UNIQUEIDENTIFIER = 'F0000005-0000-0000-0000-000000000001';
DECLARE @MsgCampaign2 UNIQUEIDENTIFIER = 'F0000005-0000-0000-0000-000000000002';

INSERT INTO MessageCampaigns (Id, ChurchId, Title, Body, Channel, [Status], ScheduledAt, SentAt, SentByMemberId, RecipientCount, DeliveredCount, FailedCount, CreatedAt, IsDeleted) VALUES
(@MsgCampaign1, @ParentChurchId, N'Invitation Conférence de Pâques', N'Chers frères et sœurs en Christ, vous êtes cordialement invités à la conférence de Pâques du 17 au 19 avril 2026 au Centre Pullman de Kinshasa. Inscription obligatoire. Que Dieu vous bénisse abondamment !', 0, 3, '2026-04-01 08:00:00', '2026-04-01 08:05:00', @Mbr07, 28, 26, 2, @Now, 0),
(@MsgCampaign2, @ParentChurchId, N'Rappel : Dîme de mars',            N'Bien-aimés, n''oubliez pas d''apporter votre dîme ce dimanche. Malachie 3:10 — Apportez à la maison du trésor toutes les dîmes, afin qu''il y ait de la nourriture dans ma maison.',                              1, 3, '2026-03-29 10:00:00', '2026-03-29 10:05:00', @Mbr07, 28, 28, 0, @Now, 0);

INSERT INTO MessageRecipients (Id, ChurchId, CampaignId, MemberId, [Status], SentAt, DeliveredAt, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @MsgCampaign1, @Mbr01, 2, '2026-04-01 08:05:00', '2026-04-01 08:05:02', @Now, 0),
(NEWID(), @ParentChurchId, @MsgCampaign1, @Mbr09, 2, '2026-04-01 08:05:00', '2026-04-01 08:05:03', @Now, 0),
(NEWID(), @ParentChurchId, @MsgCampaign1, @Mbr19, 3, '2026-04-01 08:05:00', NULL,                   @Now, 0),
(NEWID(), @ParentChurchId, @MsgCampaign2, @Mbr25, 2, '2026-03-29 10:05:00', '2026-03-29 10:05:04', @Now, 0);

-- ============================================================================
-- 22. RENDEZ-VOUS
-- ============================================================================
INSERT INTO Appointments (Id, ChurchId, MemberId, ResponsibleMemberId, Subject, [Description], [Status], MeetingType, RequestedAt, ScheduledAt, [Location], ReminderSent10Min, ReminderSent5Min, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Mbr25, @Mbr01, N'Conseil pastoral',                N'Demande de conseils pour orientation spirituelle',              2, 0, '2026-04-05 10:00:00', '2026-04-15 14:00:00', N'Bureau du pasteur', 0, 0, @Now, 0),
(NEWID(), @ParentChurchId, @Mbr26, @Mbr01, N'Préparation au baptême',           N'Entretien avant le baptême d''eau prévu en juin',               1, 0, '2026-04-10 09:00:00', '2026-04-20 10:00:00', N'Bureau du pasteur', 0, 0, @Now, 0),
(NEWID(), @ParentChurchId, @Mbr14, @Mbr09, N'Questions sur les finances',       N'Besoin de clarification sur le relevé de contributions',        4, 1, '2026-03-20 11:00:00', '2026-03-25 15:00:00', NULL,                 1, 1, @Now, 0),
(NEWID(), @ParentChurchId, @Mbr27, @Mbr02, N'Préparation au mariage',           N'Séance d''accompagnement prénuptial (3ᵉ séance)',               2, 0, '2026-04-18 14:00:00', '2026-04-28 15:00:00', N'Salle de réunion', 0, 0, @Now, 0),
(NEWID(), @ParentChurchId, @Mbr28, @Mbr13, N'Inscription au groupe Déborah',    N'Souhaite rejoindre le département des femmes',                  1, 0, '2026-04-15 09:30:00', '2026-04-22 11:00:00', NULL,                 0, 0, @Now, 0);

-- ============================================================================
-- 23. NOTIFICATIONS
-- ============================================================================
INSERT INTO Notifications (Id, ChurchId, UserId, Title, Body, [Type], IsRead, RelatedEntityType, ActionUrl, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @UMember1,      N'Conférence de Pâques',        N'N''oubliez pas de vous inscrire à la conférence du 17 au 19 avril 2026 !', 4, 0, 'ChurchEvent',   '/events',                @Now, 0),
(NEWID(), @ParentChurchId, @UTreasurer1,   N'Nouveau don reçu',            N'Un don de 3 000 000 CDF a été enregistré pour le fonds de construction.', 1, 1, 'Contribution',  '/finance/contributions', @Now, 0),
(NEWID(), @ParentChurchId, @UChurchAdmin1, N'Ticket de support ouvert',    N'Célestin Mukendi a ouvert un ticket concernant le réseau Wi-Fi.',          0, 0, 'SupportTicket', '/it/tickets',            @Now, 0),
(NEWID(), @ParentChurchId, @UMember2,      N'Rappel — Cours de croissance', N'Leçon 3 dimanche prochain : "Le salut en Jésus-Christ". Soyez ponctuel !', 5, 0, 'GrowthSchool',  '/education/growth-school', @Now, 0),
(NEWID(), @ParentChurchId, @UDeptTreas1,   N'Dépense à valider',            N'La dépense "Location centre de conférences" est en attente d''approbation.', 3, 0, 'Expense',       '/finance/expenses',      @Now, 0),
(NEWID(), @ChildChurch1Id, @UChurchAdmin2, N'Nouveau visiteur',             N'Blaise Kibwe a visité l''antenne de Lubumbashi dimanche dernier.',           0, 1, 'Visitor',       '/members/visitors',      @Now, 0);

-- ============================================================================
-- 24. SECRÉTARIAT : DOCUMENTS, CERTIFICATS, BAPTÊMES, MARIAGES
-- ============================================================================
INSERT INTO Documents (Id, ChurchId, Title, [FileName], FileUrl, FileSize, ContentType, [Type], MemberId, UploadedByMemberId, Notes, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, N'Procès-verbal AG janvier 2026',  'pv-ag-jan-2026.pdf',         '/docs/pv-ag-jan-2026.pdf',        524288,  'application/pdf', 5, NULL, @Mbr07, N'Assemblée générale ordinaire',         @Now, 0),
(NEWID(), @ParentChurchId, N'Règlement intérieur 2026',        'reglement-interieur.pdf',    '/docs/reglement-interieur.pdf',   1048576, 'application/pdf', 0, NULL, @Mbr07, N'Version mise à jour 2026',             @Now, 0),
(NEWID(), @ParentChurchId, N'Statuts de l''association',        'statuts-eeg.pdf',            '/docs/statuts-eeg.pdf',           786432,  'application/pdf', 0, NULL, @Mbr07, N'Statuts déposés au notariat',          @Now, 0),
(NEWID(), @ParentChurchId, N'Rapport financier 2025',           'rapport-fin-2025.pdf',       '/docs/rapport-fin-2025.pdf',      655360,  'application/pdf', 5, NULL, @Mbr09, N'Rapport audité par cabinet externe',   @Now, 0);

DECLARE @CertBaptism1 UNIQUEIDENTIFIER = NEWID();
DECLARE @CertBaptism2 UNIQUEIDENTIFIER = NEWID();
INSERT INTO Certificates (Id, ChurchId, [Type], CertificateNumber, MemberId, IssuedDate, IssuedByMemberId, CreatedAt, IsDeleted) VALUES
(@CertBaptism1,  @ParentChurchId, 0, 'CERT-BAPT-2026-001', @Mbr25, '2019-09-22', @Mbr01, @Now, 0),
(@CertBaptism2,  @ParentChurchId, 0, 'CERT-BAPT-2026-002', @Mbr26, '2021-01-10', @Mbr01, @Now, 0),
(NEWID(),        @ParentChurchId, 2, 'CERT-MBR-2026-001',  @Mbr01, '2008-01-15', NULL,   @Now, 0),
(NEWID(),        @ParentChurchId, 2, 'CERT-MBR-2026-002',  @Mbr27, '2018-01-15', NULL,   @Now, 0);

INSERT INTO BaptismRecords (Id, ChurchId, MemberId, BaptismDate, OfficiantMemberId, [Location], Notes, CertificateId, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Mbr25, '2019-09-22', @Mbr01, N'Fleuve Congo — Kinshasa', N'Baptême par immersion complète', @CertBaptism1, @Now, 0),
(NEWID(), @ParentChurchId, @Mbr26, '2021-01-10', @Mbr01, N'Fleuve Congo — Kinshasa', N'Baptême par immersion complète', @CertBaptism2, @Now, 0);

DECLARE @CertMarriage1 UNIQUEIDENTIFIER = NEWID();
INSERT INTO Certificates (Id, ChurchId, [Type], CertificateNumber, MemberId, IssuedDate, IssuedByMemberId, CreatedAt, IsDeleted) VALUES
(@CertMarriage1, @ParentChurchId, 1, 'CERT-MAR-2026-001', @Mbr01, '2005-08-20', NULL, @Now, 0);

INSERT INTO MarriageRecords (Id, ChurchId, Spouse1MemberId, Spouse2MemberId, MarriageDate, OfficiantMemberId, [Location], Notes, CertificateId, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Mbr01, @Mbr02, '2005-08-20', NULL, N'Église Évangélique de la Grâce — Kinshasa', N'Cérémonie religieuse et bénédiction nuptiale', @CertMarriage1, @Now, 0);

-- ============================================================================
-- 25. ÉVANGÉLISATION
-- ============================================================================
DECLARE @EvangCamp1    UNIQUEIDENTIFIER = 'F0000006-0000-0000-0000-000000000001';
DECLARE @EvangCamp2    UNIQUEIDENTIFIER = 'F0000006-0000-0000-0000-000000000002';
DECLARE @EvangTeam1    UNIQUEIDENTIFIER = 'F0000006-1000-0000-0000-000000000001';
DECLARE @EvangTeam2    UNIQUEIDENTIFIER = 'F0000006-1000-0000-0000-000000000002';
DECLARE @EvangContact1 UNIQUEIDENTIFIER = NEWID();

INSERT INTO EvangelismCampaigns (Id, ChurchId, Name, [Description], StartDate, EndDate, [Status], GoalContacts, LeaderMemberId, Notes, CreatedAt, IsDeleted) VALUES
(@EvangCamp1, @ParentChurchId, N'Opération Moisson 2026',         N'Grande campagne d''évangélisation de quartier — porte à porte et distributions de tracts', '2026-05-01', '2026-05-31', 1, 500, @Mbr19, N'Budget de 1 500 000 CDF approuvé', @Now, 0),
(@EvangCamp2, @ChildChurch2Id, N'Goma pour Christ 2026',           N'Campagne d''évangélisation sous chapiteau avec orateur invité',                           '2026-06-10', '2026-06-15', 0, 300, @Mbr20, N'Chapiteau loué, orateur confirmé',   @Now, 0);

INSERT INTO EvangelismTeams (Id, ChurchId, CampaignId, Name, LeaderMemberId, CreatedAt, IsDeleted) VALUES
(@EvangTeam1, @ParentChurchId, @EvangCamp1, N'Équipe Alpha — Quartier Gombe',    @Mbr19, @Now, 0),
(@EvangTeam2, @ParentChurchId, @EvangCamp1, N'Équipe Béta — Quartier Bandalungwa', @Mbr12, @Now, 0);

INSERT INTO EvangelismTeamMembers (Id, ChurchId, TeamId, MemberId, JoinedDate, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @EvangTeam1, @Mbr19, '2026-04-20', @Now, 0),
(NEWID(), @ParentChurchId, @EvangTeam1, @Mbr26, '2026-04-20', @Now, 0),
(NEWID(), @ParentChurchId, @EvangTeam1, @Mbr25, '2026-04-22', @Now, 0),
(NEWID(), @ParentChurchId, @EvangTeam2, @Mbr12, '2026-04-20', @Now, 0),
(NEWID(), @ParentChurchId, @EvangTeam2, @Mbr27, '2026-04-22', @Now, 0);

INSERT INTO EvangelismContacts (Id, ChurchId, CampaignId, TeamId, FirstName, LastName, Phone, [Address], [Status], AssignedToMemberId, Notes, CreatedAt, IsDeleted) VALUES
(@EvangContact1, @ParentChurchId, @EvangCamp1, @EvangTeam1, N'Hervé',       N'Tshibuabua', '+243 820 999 111', N'Gombe, Avenue de l''Équateur',     0, @Mbr19, N'Rencontré devant la boulangerie',     @Now, 0),
(NEWID(),        @ParentChurchId, @EvangCamp1, @EvangTeam1, N'Clémentine',  N'Ndaya',      '+243 820 999 222', N'Gombe, Avenue du Port',             1, @Mbr26, N'Intéressée — a accepté une Bible',     @Now, 0),
(NEWID(),        @ParentChurchId, @EvangCamp1, @EvangTeam2, N'Désiré',      N'Mbiya',      '+243 820 999 333', N'Bandalungwa, Avenue Kabambare',    2, @Mbr12, N'A assisté au culte du 13 avril 2026', @Now, 0),
(NEWID(),        @ParentChurchId, @EvangCamp1, @EvangTeam2, N'Honorine',    N'Nsiala',     '+243 820 999 444', N'Bandalungwa, Rond-point Ngaba',    1, @Mbr27, N'A reçu un livret de L''Évangile',     @Now, 0);

INSERT INTO EvangelismFollowUps (Id, ChurchId, ContactId, Method, FollowUpDate, Notes, ConductedByMemberId, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @EvangContact1, 0, '2026-04-25', N'Visite à domicile — nous avons partagé l''Évangile de Jean', @Mbr19, @Now, 0);

-- ============================================================================
-- 26. MULTIMÉDIA
-- ============================================================================
DECLARE @Media1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Media2 UNIQUEIDENTIFIER = NEWID();

INSERT INTO MediaContents (Id, ChurchId, Title, [Description], ContentType, [Status], AccessType, Price, FileUrl, ThumbnailUrl, DurationSeconds, Tags, DownloadCount, ViewCount, AuthorMemberId, PublishedAt, CreatedAt, IsDeleted) VALUES
(@Media1, @ParentChurchId, N'Prédication — La puissance de la résurrection', N'Message de Pâques 2026 par le Pasteur Patrice Tshilumba',   1, 1, 0, NULL,    '/media/predication-paques-2026.mp4',  '/media/thumb-paques-2026.jpg', 3600, N'prédication,Pâques,résurrection', 125, 680, @Mbr01, '2026-04-01', @Now, 0),
(@Media2, @ParentChurchId, N'Album — Cantiques de la Grâce vol. 3',          N'Nouvel album de la chorale avec 12 chants originaux en lingala et français', 0, 1, 1, 8500.00, '/media/cantiques-grace-vol3.zip',     '/media/thumb-cantiques-v3.jpg', NULL, N'musique,chorale,louange',          35,  210, @Mbr21, '2026-03-15', @Now, 0),
(NEWID(), @ParentChurchId, N'Enseignement — Les fondements de la prière',    N'Série d''enseignements en 6 parties sur la prière',                           2, 1, 0, NULL,    '/media/fondements-priere-s01.mp4',    '/media/thumb-priere-s01.jpg',  2700, N'enseignement,prière,série',        42,  280, @Mbr16, '2026-02-20', @Now, 0);

INSERT INTO MediaPurchases (Id, ChurchId, ContentId, MemberId, Amount, [Status], PaymentReference, PaidAt, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Media2, @Mbr25, 8500.00, 1, 'MPESA-REF-001', @Now, @Now, 0),
(NEWID(), @ParentChurchId, @Media2, @Mbr27, 8500.00, 1, 'MPESA-REF-002', @Now, @Now, 0);

INSERT INTO MediaPromotions (Id, ChurchId, ContentId, Title, [Description], Code, DiscountPercent, StartDate, EndDate, IsActive, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, NULL, N'Promo Pâques −20 %', N'20 % de réduction sur tous les contenus payants pour Pâques', 'PAQUES2026', 20.00, '2026-04-10', '2026-04-25', 1, @Now, 0);

-- ============================================================================
-- 27. LOGISTIQUE : INVENTAIRE ET VÉHICULES
-- ============================================================================
DECLARE @InvItem1 UNIQUEIDENTIFIER = 'F0000007-0000-0000-0000-000000000001';
DECLARE @InvItem2 UNIQUEIDENTIFIER = 'F0000007-0000-0000-0000-000000000002';
DECLARE @InvItem3 UNIQUEIDENTIFIER = 'F0000007-0000-0000-0000-000000000003';

INSERT INTO InventoryItems (Id, ChurchId, Name, [Description], Category, Quantity, Unit, MinQuantity, [Location], [Status], CreatedAt, IsDeleted) VALUES
(@InvItem1, @ParentChurchId, N'Chaises pliantes',         N'Chaises en plastique blanc pour événements',   N'Mobilier',     350, N'pièces', 80, N'Entrepôt principal Gombe', 0, @Now, 0),
(@InvItem2, @ParentChurchId, N'Microphones sans fil',     N'Shure SM58 — microphones de scène',            N'Sonorisation', 8,   N'pièces',  2, N'Salle technique',          0, @Now, 0),
(@InvItem3, @ParentChurchId, N'Bibles en français',       N'Bibles Louis Segond format poche',             N'Littérature',  180, N'pièces', 50, N'Bibliothèque du temple',    0, @Now, 0);

INSERT INTO InventoryTransactions (Id, ChurchId, ItemId, [Type], QuantityChange, QuantityAfter, TransactionDate, RecordedByMemberId, Notes, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @InvItem1, 0, 350, 350, '2026-01-10', @Mbr23, N'Stock initial de chaises',             @Now, 0),
(NEWID(), @ParentChurchId, @InvItem1, 1, -80, 270, '2026-04-17', @Mbr23, N'Prêt de 80 chaises pour la conférence',@Now, 0),
(NEWID(), @ParentChurchId, @InvItem2, 0, 8,   8,   '2026-01-10', @Mbr23, N'Achat de 8 microphones Shure',         @Now, 0),
(NEWID(), @ParentChurchId, @InvItem3, 0, 200, 200, '2026-02-01', @Mbr23, N'Arrivage de 200 Bibles — Mission France',@Now, 0),
(NEWID(), @ParentChurchId, @InvItem3, 1, -20, 180, '2026-04-05', @Mbr23, N'Distribution campagne d''évangélisation', @Now, 0);

DECLARE @Vehicle1 UNIQUEIDENTIFIER = 'F0000007-2000-0000-0000-000000000001';
DECLARE @Vehicle2 UNIQUEIDENTIFIER = 'F0000007-2000-0000-0000-000000000002';

INSERT INTO Vehicles (Id, ChurchId, Make, Model, [Year], PlateNumber, Capacity, [Status], Color, CreatedAt, IsDeleted) VALUES
(@Vehicle1, @ParentChurchId, 'Toyota',   'HiAce',    2022, 'KN-5601-A', 15, 0, N'Blanc', @Now, 0),
(@Vehicle2, @ParentChurchId, 'Mitsubishi','Canter',  2020, 'KN-8844-B', 30, 0, N'Bleu',  @Now, 0);

INSERT INTO VehicleBookings (Id, ChurchId, VehicleId, DriverMemberId, Purpose, StartDateTime, EndDateTime, [Status], ApprovedByMemberId, ApprovedAt, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Vehicle1, @Mbr23, N'Transport des fidèles pour la conférence de Pâques', '2026-04-17 07:00:00', '2026-04-19 18:00:00', 1, @Mbr11, @Now, @Now, 0),
(NEWID(), @ParentChurchId, @Vehicle2, @Mbr23, N'Déplacement équipe d''évangélisation vers Masina',   '2026-05-05 08:00:00', '2026-05-05 17:00:00', 0, NULL,   NULL,  @Now, 0);

-- ============================================================================
-- 28. GESTION IT : TICKETS, INTÉGRATIONS, JOURNAUX
-- ============================================================================
DECLARE @Ticket1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Ticket2 UNIQUEIDENTIFIER = NEWID();

INSERT INTO SupportTickets (Id, ChurchId, Title, [Description], Category, [Priority], [Status], SubmittedByUserId, AssignedToUserId, CreatedAt, IsDeleted) VALUES
(@Ticket1, @ParentChurchId, N'Problème de réseau Wi-Fi au temple',                N'Le Wi-Fi ne fonctionne pas correctement depuis dimanche dernier. Les fidèles ne peuvent pas se connecter.', 0, 2, 1, @UChurchAdmin1, @UITManager1, @Now, 0),
(@Ticket2, @ParentChurchId, N'Demande d''ajout du module de dons en ligne',       N'Il serait très utile d''intégrer un système de paiement en ligne (M-Pesa) pour les contributions.',             2, 1, 0, @UTreasurer1,   NULL,         @Now, 0),
(NEWID(),  @ParentChurchId, N'Panne vidéoprojecteur salle principale',            N'Le vidéoprojecteur ne s''allume plus depuis le culte de dimanche.',                                            0, 2, 0, @UMultimedia1,  @UITManager1, @Now, 0);

INSERT INTO SupportTicketComments (Id, ChurchId, TicketId, AuthorId, Content, IsInternal, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @Ticket1, @UITManager1, N'J''ai identifié le problème : le routeur principal doit être remplacé. Commande en cours chez Vodacom.', 0, @Now, 0),
(NEWID(), @ParentChurchId, @Ticket1, @UChurchAdmin1, N'Merci Célestin, peux-tu nous tenir informés de la livraison ?', 0, @Now, 0);

INSERT INTO IntegrationConfigs (Id, ChurchId, [Service], IsEnabled, WebhookUrl, IsHealthy, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, 3, 1, 'https://hooks.eeg-grace.cd/twilio',   1, @Now, 0),
(NEWID(), @ParentChurchId, 4, 1, 'https://hooks.eeg-grace.cd/sendgrid', 1, @Now, 0),
(NEWID(), @ParentChurchId, 6, 1, 'https://hooks.eeg-grace.cd/mpesa',    1, @Now, 0);

INSERT INTO SystemLogs (Id, ChurchId, [Action], EntityType, Details, UserId, [Level], CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, 'UserLogin',        'AppUser',      N'Connexion réussie depuis 192.168.1.45',                                @UChurchAdmin1, 'Info',  @Now, 0),
(NEWID(), @ParentChurchId, 'MemberCreated',    'Member',       N'Nouveau membre Emmanuel Tshibola ajouté au registre',                   @USecretary1,   'Info',  @Now, 0),
(NEWID(), @ParentChurchId, 'ContributionAdded','Contribution', N'Contribution de 3 000 000 CDF enregistrée pour le fonds construction', @UTreasurer1,   'Info',  @Now, 0),
(NEWID(), @ParentChurchId, 'BackupFailed',     'System',       N'Échec de la sauvegarde automatique — espace disque insuffisant',       NULL,           'Error', @Now, 0);

-- ============================================================================
-- 29. ABONNEMENTS, FACTURES ET CRÉDITS SMS
-- ============================================================================
DECLARE @Sub1 UNIQUEIDENTIFIER = 'F0000008-0000-0000-0000-000000000001';

INSERT INTO Subscriptions (Id, ChurchId, [Plan], [Status], BillingCycle, Amount, Currency, AutoRenew, StartDate, EndDate, NextBillingDate, PaymentMethod, CreatedAt, IsDeleted) VALUES
(@Sub1, @ParentChurchId, 4, 1, 1, 225000.00, 'CDF', 1, '2026-01-01', '2027-01-01', '2026-05-01', 4, @Now, 0);

INSERT INTO Invoices (Id, ChurchId, InvoiceNumber, [Description], Amount, Currency, [Status], DueDate, PaidAt, PaymentMethod, SubscriptionId, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, 'INV-2026-001', N'Abonnement Premium — janvier 2026', 225000.00, 'CDF', 2, '2026-01-15', '2026-01-10', 4,    @Sub1, @Now, 0),
(NEWID(), @ParentChurchId, 'INV-2026-002', N'Abonnement Premium — février 2026', 225000.00, 'CDF', 2, '2026-02-15', '2026-02-12', 4,    @Sub1, @Now, 0),
(NEWID(), @ParentChurchId, 'INV-2026-003', N'Abonnement Premium — mars 2026',    225000.00, 'CDF', 2, '2026-03-15', '2026-03-10', 4,    @Sub1, @Now, 0),
(NEWID(), @ParentChurchId, 'INV-2026-004', N'Abonnement Premium — avril 2026',   225000.00, 'CDF', 1, '2026-04-15', NULL,          NULL, @Sub1, @Now, 0);

DECLARE @SmsCredit1 UNIQUEIDENTIFIER = 'F0000008-1000-0000-0000-000000000001';

INSERT INTO SmsCredits (Id, ChurchId, Balance, TotalPurchased, TotalConsumed, CreatedAt, IsDeleted) VALUES
(@SmsCredit1, @ParentChurchId, 940, 1000, 60, @Now, 0);

INSERT INTO SmsCreditTransactions (Id, ChurchId, SmsCreditId, [Type], Amount, BalanceBefore, BalanceAfter, Reference, Notes, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, @SmsCredit1, 0, 1000,  0,    1000, 'PAY-SMS-2026-001', N'Achat de 1 000 crédits SMS',                  @Now, 0),
(NEWID(), @ParentChurchId, @SmsCredit1, 1, -28,   1000, 972,  'CAMP-MSG-001',     N'Envoi campagne conférence de Pâques',          @Now, 0),
(NEWID(), @ParentChurchId, @SmsCredit1, 1, -32,   972,  940,  'CAMP-MSG-002',     N'Envoi rappel dîme de mars',                     @Now, 0);

-- ============================================================================
-- 30. CHAMPS PERSONNALISÉS
-- ============================================================================
INSERT INTO CustomFields (Id, ChurchId, Name, Label, FieldType, IsRequired, DisplayOrder, Options, CreatedAt, IsDeleted) VALUES
(NEWID(), @ParentChurchId, 'emergency_contact', N'Contact d''urgence',            0, 0, 1, NULL, @Now, 0),
(NEWID(), @ParentChurchId, 'allergies',         N'Allergies connues',             0, 0, 2, NULL, @Now, 0),
(NEWID(), @ParentChurchId, 'spiritual_gift',    N'Don spirituel principal',       4, 0, 3, N'["Enseignement","Prophétie","Guérison","Service","Sagesse","Évangélisation","Hospitalité"]', @Now, 0),
(NEWID(), @ParentChurchId, 'mother_tongue',     N'Langue maternelle',             4, 0, 4, N'["Français","Lingala","Swahili","Tshiluba","Kikongo"]', @Now, 0);

COMMIT TRANSACTION;
SET NOEXEC OFF;
PRINT N'✓ Données de seed insérées avec succès — toutes les tables sont peuplées.';
