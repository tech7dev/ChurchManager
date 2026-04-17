using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ChurchMS.BlazorAdmin.Services;

/// <summary>
/// Manages UI language selection and provides translated strings.
/// Supported: en (English), fr (French), sw (Swahili), ln (Lingala).
/// </summary>
public class LocalisationService
{
    private const string StorageKey = "churchms_lang";
    private readonly ProtectedLocalStorage _storage;
    private string _currentLang = "fr";

    public event Action? OnLanguageChanged;

    public LocalisationService(ProtectedLocalStorage storage)
    {
        _storage = storage;
    }

    public string CurrentLang => _currentLang;

    public string CurrentLangLabel => _currentLang switch
    {
        "fr" => "FR",
        "en" => "EN",
        "sw" => "SW",
        "ln" => "LN",
        _ => "FR"
    };

    public string CurrentLangFlag => _currentLang switch
    {
        "fr" => "\U0001F1EB\U0001F1F7",
        "en" => "\U0001F1EC\U0001F1E7",
        "sw" => "\U0001F1F9\U0001F1FF",
        "ln" => "\U0001F1E8\U0001F1E9",
        _ => "\U0001F1EB\U0001F1F7"
    };

    public static readonly (string Code, string Label, string Flag)[] SupportedLanguages =
    [
        ("fr", "Fran\u00e7ais", "\U0001F1EB\U0001F1F7"),
        ("en", "English", "\U0001F1EC\U0001F1E7"),
        ("sw", "Kiswahili", "\U0001F1F9\U0001F1FF"),
        ("ln", "Ling\u00e1la", "\U0001F1E8\U0001F1E9"),
    ];

    public async Task InitializeAsync()
    {
        try
        {
            var result = await _storage.GetAsync<string>(StorageKey);
            if (result.Success && !string.IsNullOrWhiteSpace(result.Value))
                _currentLang = result.Value;
        }
        catch { /* first load — keep default */ }
    }

    public async Task SetLanguageAsync(string langCode)
    {
        if (_currentLang == langCode) return;
        _currentLang = langCode;
        await _storage.SetAsync(StorageKey, langCode);
        OnLanguageChanged?.Invoke();
    }

    /// <summary>
    /// Get a translated string by key. Falls back to English, then returns the key itself.
    /// </summary>
    public string T(string key)
    {
        if (Translations.TryGetValue(key, out var langs))
        {
            if (langs.TryGetValue(_currentLang, out var val))
                return val;
            if (langs.TryGetValue("en", out var enVal))
                return enVal;
        }
        return key;
    }

    // ========================================================================
    // Translation dictionary — all UI strings
    // ========================================================================
    private static readonly Dictionary<string, Dictionary<string, string>> Translations = new()
    {
        // ── App chrome ──────────────────────────────────────────────────
        ["app.name"] = new() { ["fr"] = "ChurchMS", ["en"] = "ChurchMS", ["sw"] = "ChurchMS", ["ln"] = "ChurchMS" },
        ["app.admin"] = new() { ["fr"] = "Admin", ["en"] = "Admin", ["sw"] = "Msimamizi", ["ln"] = "Mokonzi" },
        ["app.signout"] = new() { ["fr"] = "D\u00e9connexion", ["en"] = "Sign Out", ["sw"] = "Ondoka", ["ln"] = "Kobima" },
        ["app.profile"] = new() { ["fr"] = "Profil", ["en"] = "Profile", ["sw"] = "Wasifu", ["ln"] = "Profil" },
        ["app.settings"] = new() { ["fr"] = "Param\u00e8tres", ["en"] = "Settings", ["sw"] = "Mipangilio", ["ln"] = "Mipangilio" },
        ["app.language"] = new() { ["fr"] = "Langue", ["en"] = "Language", ["sw"] = "Lugha", ["ln"] = "Monoko" },

        // ── Login ───────────────────────────────────────────────────────
        ["login.title"] = new() { ["fr"] = "Bienvenue", ["en"] = "Welcome back", ["sw"] = "Karibu tena", ["ln"] = "Boyei malamu" },
        ["login.subtitle"] = new() { ["fr"] = "Connectez-vous \u00e0 votre compte", ["en"] = "Sign in to your admin account", ["sw"] = "Ingia kwenye akaunti yako", ["ln"] = "Kota na compte na yo" },
        ["login.email"] = new() { ["fr"] = "Adresse e-mail", ["en"] = "Email address", ["sw"] = "Barua pepe", ["ln"] = "Adr\u00e8se email" },
        ["login.password"] = new() { ["fr"] = "Mot de passe", ["en"] = "Password", ["sw"] = "Nenosiri", ["ln"] = "Mot de passe" },
        ["login.submit"] = new() { ["fr"] = "Se connecter", ["en"] = "Sign In", ["sw"] = "Ingia", ["ln"] = "Kota" },
        ["login.loading"] = new() { ["fr"] = "Connexion\u2026", ["en"] = "Signing in\u2026", ["sw"] = "Inaingia\u2026", ["ln"] = "Kokota\u2026" },
        ["login.error"] = new() { ["fr"] = "Identifiants invalides. Veuillez r\u00e9essayer.", ["en"] = "Invalid credentials. Please try again.", ["sw"] = "Sifa batili. Tafadhali jaribu tena.", ["ln"] = "Mot de passe to ezali malamu te. M\u00e9ki lisusu." },

        // ── Nav groups ──────────────────────────────────────────────────
        ["nav.dashboard"] = new() { ["fr"] = "Tableau de bord", ["en"] = "Dashboard", ["sw"] = "Dashibodi", ["ln"] = "Tableau ya bord" },
        ["nav.members"] = new() { ["fr"] = "Membres", ["en"] = "Members", ["sw"] = "Wanachama", ["ln"] = "Bandimi" },
        ["nav.members.all"] = new() { ["fr"] = "Tous les membres", ["en"] = "All Members", ["sw"] = "Wanachama wote", ["ln"] = "Bandimi nyonso" },
        ["nav.visitors"] = new() { ["fr"] = "Visiteurs", ["en"] = "Visitors", ["sw"] = "Wageni", ["ln"] = "Bapaya" },
        ["nav.families"] = new() { ["fr"] = "Familles", ["en"] = "Families", ["sw"] = "Familia", ["ln"] = "Mabota" },
        ["nav.communication"] = new() { ["fr"] = "Communication", ["en"] = "Communication", ["sw"] = "Mawasiliano", ["ln"] = "Lisango" },
        ["nav.campaigns"] = new() { ["fr"] = "Campagnes", ["en"] = "Campaigns", ["sw"] = "Kampeni", ["ln"] = "Bakampanye" },
        ["nav.appointments"] = new() { ["fr"] = "Rendez-vous", ["en"] = "Appointments", ["sw"] = "Miadi", ["ln"] = "Bakita" },
        ["nav.notifications"] = new() { ["fr"] = "Notifications", ["en"] = "Notifications", ["sw"] = "Arifa", ["ln"] = "Bansango" },
        ["nav.finance"] = new() { ["fr"] = "Finances", ["en"] = "Finance", ["sw"] = "Fedha", ["ln"] = "Mbongo" },
        ["nav.contributions"] = new() { ["fr"] = "Contributions", ["en"] = "Contributions", ["sw"] = "Michango", ["ln"] = "Makabo" },
        ["nav.expenses"] = new() { ["fr"] = "D\u00e9penses", ["en"] = "Expenses", ["sw"] = "Gharama", ["ln"] = "Bifut\u00e9li" },
        ["nav.funds"] = new() { ["fr"] = "Fonds", ["en"] = "Funds", ["sw"] = "Fedha", ["ln"] = "Mifundu" },
        ["nav.contrib.campaigns"] = new() { ["fr"] = "Campagnes", ["en"] = "Campaigns", ["sw"] = "Kampeni", ["ln"] = "Bakampanye" },
        ["nav.budgets"] = new() { ["fr"] = "Budgets", ["en"] = "Budgets", ["sw"] = "Bajeti", ["ln"] = "Budg\u00e9" },
        ["nav.events"] = new() { ["fr"] = "\u00c9v\u00e9nements", ["en"] = "Events", ["sw"] = "Matukio", ["ln"] = "Makambo" },
        ["nav.events.all"] = new() { ["fr"] = "Tous les \u00e9v\u00e9nements", ["en"] = "All Events", ["sw"] = "Matukio yote", ["ln"] = "Makambo nyonso" },
        ["nav.attendance"] = new() { ["fr"] = "Pr\u00e9sences", ["en"] = "Attendance", ["sw"] = "Mahudhurio", ["ln"] = "Kozala" },
        ["nav.education"] = new() { ["fr"] = "\u00c9ducation", ["en"] = "Education", ["sw"] = "Elimu", ["ln"] = "Boyekoli" },
        ["nav.sundayschool"] = new() { ["fr"] = "\u00c9cole du dimanche", ["en"] = "Sunday School", ["sw"] = "Shule ya Jumapili", ["ln"] = "Kel\u00e1si ya Eyenga" },
        ["nav.growthschool"] = new() { ["fr"] = "\u00c9cole de croissance", ["en"] = "Growth School", ["sw"] = "Shule ya Ukuaji", ["ln"] = "Kel\u00e1si ya kokola" },
        ["nav.ministry"] = new() { ["fr"] = "Minist\u00e8res", ["en"] = "Ministry", ["sw"] = "Huduma", ["ln"] = "Misala" },
        ["nav.departments"] = new() { ["fr"] = "D\u00e9partements", ["en"] = "Departments", ["sw"] = "Idara", ["ln"] = "Biteni" },
        ["nav.evangelism"] = new() { ["fr"] = "\u00c9vang\u00e9lisation", ["en"] = "Evangelism", ["sw"] = "Uinjilisti", ["ln"] = "Kosakola" },
        ["nav.secretariat"] = new() { ["fr"] = "Secr\u00e9tariat", ["en"] = "Secretariat", ["sw"] = "Sekretarieti", ["ln"] = "Sekretar\u00eda" },
        ["nav.multimedia"] = new() { ["fr"] = "Multim\u00e9dia", ["en"] = "Multimedia", ["sw"] = "Vyombo", ["ln"] = "Multim\u00e9dia" },
        ["nav.logistics"] = new() { ["fr"] = "Logistique", ["en"] = "Logistics", ["sw"] = "Vifaa", ["ln"] = "Logistike" },
        ["nav.reports"] = new() { ["fr"] = "Rapports", ["en"] = "Reports", ["sw"] = "Ripoti", ["ln"] = "Baraporo" },
        ["nav.reports.financial"] = new() { ["fr"] = "Rapport financier", ["en"] = "Financial", ["sw"] = "Fedha", ["ln"] = "Ya mbongo" },
        ["nav.reports.members"] = new() { ["fr"] = "Rapport des membres", ["en"] = "Members", ["sw"] = "Wanachama", ["ln"] = "Ya bandimi" },
        ["nav.reports.attendance"] = new() { ["fr"] = "Rapport de pr\u00e9sences", ["en"] = "Attendance", ["sw"] = "Mahudhurio", ["ln"] = "Ya kozala" },
        ["nav.administration"] = new() { ["fr"] = "Administration", ["en"] = "Administration", ["sw"] = "Utawala", ["ln"] = "Boyangeli" },
        ["nav.subscription"] = new() { ["fr"] = "Abonnement", ["en"] = "Subscription", ["sw"] = "Usajili", ["ln"] = "Abonema" },
        ["nav.users"] = new() { ["fr"] = "Utilisateurs", ["en"] = "Users", ["sw"] = "Watumiaji", ["ln"] = "Basaleli" },
        ["nav.tickets"] = new() { ["fr"] = "Tickets de support", ["en"] = "Support Tickets", ["sw"] = "Tiketi", ["ln"] = "Batik\u00e8" },
        ["nav.integrations"] = new() { ["fr"] = "Int\u00e9grations", ["en"] = "Integrations", ["sw"] = "Muunganisho", ["ln"] = "Ba int\u00e9gration" },

        // ── Page titles & subtitles ─────────────────────────────────────
        ["page.dashboard.title"] = new() { ["fr"] = "Tableau de bord", ["en"] = "Dashboard", ["sw"] = "Dashibodi", ["ln"] = "Tableau ya bord" },
        ["page.dashboard.subtitle"] = new() { ["fr"] = "Vue d'ensemble de votre \u00e9glise", ["en"] = "Overview of your church", ["sw"] = "Muhtasari wa kanisa lako", ["ln"] = "Botaleli ya Ndako na Nzambe" },
        ["page.members.title"] = new() { ["fr"] = "Membres", ["en"] = "Members", ["sw"] = "Wanachama", ["ln"] = "Bandimi" },
        ["page.members.subtitle"] = new() { ["fr"] = "G\u00e9rer les membres de l'\u00e9glise", ["en"] = "Manage church members and their details", ["sw"] = "Simamia wanachama wa kanisa", ["ln"] = "Bomb\u00e1 bandimi ya Ndako na Nzambe" },
        ["page.members.add"] = new() { ["fr"] = "Ajouter un membre", ["en"] = "Add Member", ["sw"] = "Ongeza Mwanachama", ["ln"] = "B\u00e1kisa mondimi" },
        ["page.members.search"] = new() { ["fr"] = "Rechercher par nom, t\u00e9l\u00e9phone, e-mail\u2026", ["en"] = "Search by name, phone, email or membership number\u2026", ["sw"] = "Tafuta kwa jina, simu, barua pepe\u2026", ["ln"] = "Luka na nkombo, t\u00e9l\u00e9phone, email\u2026" },
        ["page.visitors.title"] = new() { ["fr"] = "Visiteurs", ["en"] = "Visitors", ["sw"] = "Wageni", ["ln"] = "Bapaya" },
        ["page.visitors.subtitle"] = new() { ["fr"] = "Suivre et accompagner les visiteurs", ["en"] = "Track and follow up with church visitors", ["sw"] = "Fuatilia wageni wa kanisa", ["ln"] = "Kolanda bapaya" },
        ["page.families.title"] = new() { ["fr"] = "Familles", ["en"] = "Families", ["sw"] = "Familia", ["ln"] = "Mabota" },
        ["page.families.subtitle"] = new() { ["fr"] = "G\u00e9rer les unit\u00e9s familiales", ["en"] = "Manage family units and relationships", ["sw"] = "Simamia familia na mahusiano", ["ln"] = "Bomb\u00e1 mabota" },
        ["page.contributions.title"] = new() { ["fr"] = "Contributions", ["en"] = "Contributions", ["sw"] = "Michango", ["ln"] = "Makabo" },
        ["page.contributions.subtitle"] = new() { ["fr"] = "D\u00eemes, offrandes et dons", ["en"] = "Tithes, offerings, and donations", ["sw"] = "Zaka, sadaka, na michango", ["ln"] = "Zaka, mab\u00f3nza, na makabo" },
        ["page.expenses.title"] = new() { ["fr"] = "D\u00e9penses", ["en"] = "Expenses", ["sw"] = "Gharama", ["ln"] = "Bifut\u00e9li" },
        ["page.expenses.subtitle"] = new() { ["fr"] = "Suivi des d\u00e9penses et factures", ["en"] = "Track expenses and invoices", ["sw"] = "Fuatilia gharama na ankara", ["ln"] = "Kolanda bifut\u00e9li" },
        ["page.funds.title"] = new() { ["fr"] = "Fonds", ["en"] = "Funds", ["sw"] = "Fedha", ["ln"] = "Mifundu" },
        ["page.funds.subtitle"] = new() { ["fr"] = "G\u00e9rer les fonds de contributions", ["en"] = "Manage contribution funds and designations", ["sw"] = "Simamia mifuko ya michango", ["ln"] = "Bomb\u00e1 mifundu ya makabo" },
        ["page.campaigns.title"] = new() { ["fr"] = "Campagnes de contribution", ["en"] = "Contribution Campaigns", ["sw"] = "Kampeni za Michango", ["ln"] = "Bakampanye ya makabo" },
        ["page.campaigns.subtitle"] = new() { ["fr"] = "Suivre les campagnes de collecte", ["en"] = "Track fundraising campaigns and goals", ["sw"] = "Fuatilia kampeni za kuchangia", ["ln"] = "Kolanda bakampanye" },
        ["page.budgets.title"] = new() { ["fr"] = "Budgets", ["en"] = "Budgets", ["sw"] = "Bajeti", ["ln"] = "Budg\u00e9" },
        ["page.budgets.subtitle"] = new() { ["fr"] = "Budgets annuels et suivi", ["en"] = "Annual budgets and expense tracking", ["sw"] = "Bajeti za mwaka na ufuatiliaji", ["ln"] = "Budg\u00e9 ya mbula na kolanda" },
        ["page.events.title"] = new() { ["fr"] = "\u00c9v\u00e9nements", ["en"] = "Events", ["sw"] = "Matukio", ["ln"] = "Makambo" },
        ["page.events.subtitle"] = new() { ["fr"] = "G\u00e9rer les cultes, conf\u00e9rences et \u00e9v\u00e9nements", ["en"] = "Manage services, conferences, and church events", ["sw"] = "Simamia ibada, mikutano na matukio", ["ln"] = "Bomb\u00e1 losambo, bakongr\u00e8, na makambo" },
        ["page.attendance.title"] = new() { ["fr"] = "Pr\u00e9sences", ["en"] = "Attendance", ["sw"] = "Mahudhurio", ["ln"] = "Kozala" },
        ["page.attendance.subtitle"] = new() { ["fr"] = "Suivi des pr\u00e9sences aux \u00e9v\u00e9nements", ["en"] = "Track event attendance records", ["sw"] = "Fuatilia mahudhurio ya matukio", ["ln"] = "Kolanda bato baye bayaki" },
        ["page.sundayschool.title"] = new() { ["fr"] = "\u00c9cole du dimanche", ["en"] = "Sunday School", ["sw"] = "Shule ya Jumapili", ["ln"] = "Kel\u00e1si ya Eyenga" },
        ["page.sundayschool.subtitle"] = new() { ["fr"] = "Classes, le\u00e7ons et inscriptions", ["en"] = "Manage classes, lessons, and student enrollment", ["sw"] = "Simamia madarasa, masomo na usajili", ["ln"] = "Bomb\u00e1 bakel\u00e1si, mateya na bokomi" },
        ["page.growthschool.title"] = new() { ["fr"] = "\u00c9cole de croissance", ["en"] = "Growth School", ["sw"] = "Shule ya Ukuaji", ["ln"] = "Kel\u00e1si ya kokola" },
        ["page.growthschool.subtitle"] = new() { ["fr"] = "Cours de discipulat et croissance spirituelle", ["en"] = "Discipleship courses and spiritual growth programs", ["sw"] = "Kozi za uanafunzi na ukuaji wa kiroho", ["ln"] = "Ba cours ya boyekoli na kokola na molimo" },
        ["page.departments.title"] = new() { ["fr"] = "D\u00e9partements", ["en"] = "Departments", ["sw"] = "Idara", ["ln"] = "Biteni" },
        ["page.departments.subtitle"] = new() { ["fr"] = "G\u00e9rer les d\u00e9partements et \u00e9quipes", ["en"] = "Manage church departments and ministry teams", ["sw"] = "Simamia idara na timu za huduma", ["ln"] = "Bomb\u00e1 biteni na ba \u00e9quipe" },
        ["page.evangelism.title"] = new() { ["fr"] = "\u00c9vang\u00e9lisation", ["en"] = "Evangelism", ["sw"] = "Uinjilisti", ["ln"] = "Kosakola" },
        ["page.evangelism.subtitle"] = new() { ["fr"] = "Campagnes, \u00e9quipes et suivi des contacts", ["en"] = "Outreach campaigns, teams, and contact follow-ups", ["sw"] = "Kampeni, timu na ufuatiliaji", ["ln"] = "Bakampanye, ba \u00e9quipe na kolanda" },
        ["page.secretariat.title"] = new() { ["fr"] = "Secr\u00e9tariat", ["en"] = "Secretariat", ["sw"] = "Sekretarieti", ["ln"] = "Sekretar\u00eda" },
        ["page.secretariat.subtitle"] = new() { ["fr"] = "Documents, certificats, bapt\u00eames et mariages", ["en"] = "Documents, certificates, baptism and marriage records", ["sw"] = "Nyaraka, vyeti, ubatizo na ndoa", ["ln"] = "Mikanda, ba certificat, batisimo na libala" },
        ["page.multimedia.title"] = new() { ["fr"] = "Multim\u00e9dia", ["en"] = "Multimedia", ["sw"] = "Vyombo", ["ln"] = "Multim\u00e9dia" },
        ["page.multimedia.subtitle"] = new() { ["fr"] = "G\u00e9rer les contenus vid\u00e9o, audio et num\u00e9riques", ["en"] = "Manage video, audio, and digital content", ["sw"] = "Simamia video, sauti na maudhui", ["ln"] = "Bomb\u00e1 vid\u00e9o, loyembo na makambo ya num\u00e9rique" },
        ["page.logistics.title"] = new() { ["fr"] = "Logistique", ["en"] = "Logistics", ["sw"] = "Vifaa", ["ln"] = "Logistike" },
        ["page.logistics.subtitle"] = new() { ["fr"] = "Gestion des stocks et du parc automobile", ["en"] = "Inventory management and vehicle fleet", ["sw"] = "Usimamizi wa vifaa na magari", ["ln"] = "Kobomb\u00e1 biloko na ba motuka" },
        ["page.reports.financial.title"] = new() { ["fr"] = "Rapport financier", ["en"] = "Financial Report", ["sw"] = "Ripoti ya Fedha", ["ln"] = "Raporo ya mbongo" },
        ["page.reports.members.title"] = new() { ["fr"] = "Rapport des membres", ["en"] = "Member Report", ["sw"] = "Ripoti ya Wanachama", ["ln"] = "Raporo ya bandimi" },
        ["page.reports.attendance.title"] = new() { ["fr"] = "Rapport de pr\u00e9sences", ["en"] = "Attendance Report", ["sw"] = "Ripoti ya Mahudhurio", ["ln"] = "Raporo ya kozala" },
        ["page.subscription.title"] = new() { ["fr"] = "Abonnement et facturation", ["en"] = "Subscription & Billing", ["sw"] = "Usajili na Malipo", ["ln"] = "Abonema na bafakture" },
        ["page.subscription.subtitle"] = new() { ["fr"] = "G\u00e9rer votre abonnement et factures", ["en"] = "Manage your church subscription plan and invoices", ["sw"] = "Simamia mpango wako na ankara", ["ln"] = "Bomb\u00e1 abonema na bafakture" },
        ["page.users.title"] = new() { ["fr"] = "Utilisateurs", ["en"] = "Users", ["sw"] = "Watumiaji", ["ln"] = "Basaleli" },
        ["page.users.subtitle"] = new() { ["fr"] = "G\u00e9rer les comptes et les r\u00f4les", ["en"] = "Manage user accounts and role assignments", ["sw"] = "Simamia akaunti na majukumu", ["ln"] = "Bomb\u00e1 ba compte na misala" },
        ["page.tickets.title"] = new() { ["fr"] = "Tickets de support", ["en"] = "Support Tickets", ["sw"] = "Tiketi za Msaada", ["ln"] = "Batik\u00e8 ya lisalisi" },
        ["page.tickets.subtitle"] = new() { ["fr"] = "Suivre et r\u00e9soudre les demandes", ["en"] = "Track and resolve support requests", ["sw"] = "Fuatilia na kutatua maombi", ["ln"] = "Kolanda na kosilisa bademande" },
        ["page.integrations.title"] = new() { ["fr"] = "Int\u00e9grations", ["en"] = "Integrations", ["sw"] = "Muunganisho", ["ln"] = "Ba int\u00e9gration" },
        ["page.integrations.subtitle"] = new() { ["fr"] = "Configurer les services externes", ["en"] = "Configure external service connections", ["sw"] = "Sanidi miunganisho ya nje", ["ln"] = "Kobongisa ba service ya libanda" },
        ["page.msgcampaigns.title"] = new() { ["fr"] = "Campagnes de messages", ["en"] = "Message Campaigns", ["sw"] = "Kampeni za Ujumbe", ["ln"] = "Bakampanye ya bansango" },
        ["page.msgcampaigns.subtitle"] = new() { ["fr"] = "Envoyer des SMS, e-mails et WhatsApp", ["en"] = "Send SMS, email, and WhatsApp campaigns", ["sw"] = "Tuma SMS, barua pepe na WhatsApp", ["ln"] = "Kotinda SMS, email na WhatsApp" },
        ["page.appointments.title"] = new() { ["fr"] = "Rendez-vous", ["en"] = "Appointments", ["sw"] = "Miadi", ["ln"] = "Bakita" },
        ["page.appointments.subtitle"] = new() { ["fr"] = "Planifier et g\u00e9rer les rendez-vous pastoraux", ["en"] = "Schedule and manage pastoral appointments", ["sw"] = "Panga na simamia miadi ya kichungaji", ["ln"] = "Kobongisa na kobomb\u00e1 bakita" },
        ["page.notifications.title"] = new() { ["fr"] = "Notifications", ["en"] = "Notifications", ["sw"] = "Arifa", ["ln"] = "Bansango" },
        ["page.notifications.subtitle"] = new() { ["fr"] = "Alertes et rappels du syst\u00e8me", ["en"] = "System alerts and reminders", ["sw"] = "Tahadhari na vikumbusho", ["ln"] = "Ba alerte na bakaniseli" },

        // ── Common table / UI labels ────────────────────────────────────
        ["common.name"] = new() { ["fr"] = "Nom", ["en"] = "Name", ["sw"] = "Jina", ["ln"] = "Nkombo" },
        ["common.email"] = new() { ["fr"] = "E-mail", ["en"] = "Email", ["sw"] = "Barua pepe", ["ln"] = "Email" },
        ["common.phone"] = new() { ["fr"] = "T\u00e9l\u00e9phone", ["en"] = "Phone", ["sw"] = "Simu", ["ln"] = "T\u00e9l\u00e9phone" },
        ["common.status"] = new() { ["fr"] = "Statut", ["en"] = "Status", ["sw"] = "Hali", ["ln"] = "Ezaleli" },
        ["common.date"] = new() { ["fr"] = "Date", ["en"] = "Date", ["sw"] = "Tarehe", ["ln"] = "Mokolo" },
        ["common.actions"] = new() { ["fr"] = "Actions", ["en"] = "Actions", ["sw"] = "Vitendo", ["ln"] = "Misala" },
        ["common.search"] = new() { ["fr"] = "Rechercher\u2026", ["en"] = "Search\u2026", ["sw"] = "Tafuta\u2026", ["ln"] = "Luka\u2026" },
        ["common.norecords"] = new() { ["fr"] = "Aucun enregistrement trouv\u00e9.", ["en"] = "No records found.", ["sw"] = "Hakuna rekodi zilizopatikana.", ["ln"] = "Eloko moko te emonani." },
        ["common.loading"] = new() { ["fr"] = "Chargement\u2026", ["en"] = "Loading\u2026", ["sw"] = "Inapakia\u2026", ["ln"] = "Ezali koya\u2026" },
        ["common.active"] = new() { ["fr"] = "Actif", ["en"] = "Active", ["sw"] = "Hai", ["ln"] = "Ezali kosala" },
        ["common.inactive"] = new() { ["fr"] = "Inactif", ["en"] = "Inactive", ["sw"] = "Haifanyi kazi", ["ln"] = "Ezali kosala te" },
        ["common.yes"] = new() { ["fr"] = "Oui", ["en"] = "Yes", ["sw"] = "Ndiyo", ["ln"] = "Iyo" },
        ["common.no"] = new() { ["fr"] = "Non", ["en"] = "No", ["sw"] = "Hapana", ["ln"] = "Te" },
        ["common.save"] = new() { ["fr"] = "Enregistrer", ["en"] = "Save", ["sw"] = "Hifadhi", ["ln"] = "Kobomba" },
        ["common.cancel"] = new() { ["fr"] = "Annuler", ["en"] = "Cancel", ["sw"] = "Ghairi", ["ln"] = "Koboya" },
        ["common.delete"] = new() { ["fr"] = "Supprimer", ["en"] = "Delete", ["sw"] = "Futa", ["ln"] = "Kolongola" },
        ["common.edit"] = new() { ["fr"] = "Modifier", ["en"] = "Edit", ["sw"] = "Hariri", ["ln"] = "Kobongola" },
        ["common.add"] = new() { ["fr"] = "Ajouter", ["en"] = "Add", ["sw"] = "Ongeza", ["ln"] = "B\u00e1kisa" },
        ["common.type"] = new() { ["fr"] = "Type", ["en"] = "Type", ["sw"] = "Aina", ["ln"] = "Lolenge" },
        ["common.description"] = new() { ["fr"] = "Description", ["en"] = "Description", ["sw"] = "Maelezo", ["ln"] = "Ndimbola" },
        ["common.amount"] = new() { ["fr"] = "Montant", ["en"] = "Amount", ["sw"] = "Kiasi", ["ln"] = "Motango" },
        ["common.title"] = new() { ["fr"] = "Titre", ["en"] = "Title", ["sw"] = "Kichwa", ["ln"] = "Nkombo" },

        // ── Tab labels ──────────────────────────────────────────────────
        ["tab.documents"] = new() { ["fr"] = "Documents", ["en"] = "Documents", ["sw"] = "Nyaraka", ["ln"] = "Mikanda" },
        ["tab.certificates"] = new() { ["fr"] = "Certificats", ["en"] = "Certificates", ["sw"] = "Vyeti", ["ln"] = "Ba certificat" },
        ["tab.inventory"] = new() { ["fr"] = "Inventaire", ["en"] = "Inventory", ["sw"] = "Hesabu", ["ln"] = "Inventaire" },
        ["tab.vehicles"] = new() { ["fr"] = "V\u00e9hicules", ["en"] = "Vehicles", ["sw"] = "Magari", ["ln"] = "Ba motuka" },
    };
}
