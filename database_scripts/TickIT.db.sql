BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS Comments (
    CommentID INTEGER PRIMARY KEY AUTOINCREMENT,
    TicketID INTEGER NOT NULL,
    UserID INTEGER NOT NULL,
    CommentText TEXT NOT NULL,
    CreatedDate TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (TicketID) REFERENCES Tickets(TicketID) ON DELETE CASCADE,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS Priorities (
    PriorityID INTEGER PRIMARY KEY AUTOINCREMENT,
    PriorityName TEXT UNIQUE NOT NULL
);
CREATE TABLE IF NOT EXISTS ReopenReasons (
    ReasonID INTEGER PRIMARY KEY AUTOINCREMENT,
    ReasonText TEXT NOT NULL UNIQUE
);
CREATE TABLE IF NOT EXISTS Statuses (
    StatusID INTEGER PRIMARY KEY AUTOINCREMENT,
    StatusName TEXT UNIQUE NOT NULL
);
CREATE TABLE IF NOT EXISTS Tickets (
    TicketID INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Description TEXT NOT NULL,
    UserID INTEGER NOT NULL,
    TechnicianID INTEGER NULL,
    StatusID INTEGER NOT NULL,
    PriorityID INTEGER NOT NULL,
    CreatedDate TEXT DEFAULT CURRENT_TIMESTAMP,
    ResolvedDate TEXT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
    FOREIGN KEY (TechnicianID) REFERENCES Users(UserID) ON DELETE SET NULL,
    FOREIGN KEY (StatusID) REFERENCES Statuses(StatusID) ON DELETE CASCADE,
    FOREIGN KEY (PriorityID) REFERENCES Priorities(PriorityID) ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS Users (
    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
    Email TEXT UNIQUE NOT NULL,
    Phone TEXT NULL,
    Role TEXT CHECK (Role IN ('User', 'Technician', 'Admin')) NOT NULL,
    PasswordHash TEXT NOT NULL
, FirstName TEXT, LastName TEXT, IsPasswordChangeRequired INTEGER DEFAULT 0, IsInactive INTEGER DEFAULT 0);
INSERT INTO "Comments" ("CommentID","TicketID","UserID","CommentText","CreatedDate") VALUES (1,1,3,'Sprawdzam problem, dam znać wkrótce.','2025-03-22 15:24:27'),
 (2,2,3,'adasdadad','2025-05-08 12:20:18'),
 (4,1,2,'Sprawdzono zasilacz – wygląda OK. Możliwe uszkodzenie płyty.','2025-04-26 07:08:24'),
 (6,2,4,'Zrestartowano routery. Użytkownik nadal nie ma dostępu.','2025-04-26 07:08:24'),
 (10,5,3,'Po aktualizacji nadal są błędy przy zapisie dokumentu.','2025-04-26 07:08:24'),
 (15,1,3,'Sprawdzono konfigurację sieci – wszystko wygląda poprawnie.','2025-05-01 09:00:00'),
 (16,2,4,'Zidentyfikowano problem w kodzie aplikacji, przekazano do deweloperów.','2025-05-02 11:30:00'),
 (17,3,3,'Reset hasła użytkownika nie pomógł – analizujemy dalej.','2025-05-04 10:20:00'),
 (18,4,5,'Sprzęt działał poprawnie jeszcze wczoraj – możliwa awaria sprzętowa.','2025-05-05 12:00:00'),
 (19,5,1,'Utworzono konto dla nowego użytkownika.','2025-05-06 08:30:00'),
 (20,3,3,'znowu nie działa','2025-05-08 13:23:08'),
 (21,14,1,'IWA is not working for me I can only use teams and outlook but no website can be opened by me','2025-05-08 14:00:31.8840084'),
 (22,15,1,'po urlopie straciła dostep do dokumentów w wikihr.pl','2025-05-08 14:09:57.5774347'),
 (23,11,3,'nie wiem co sie stało, sprawdzam problem','2025-05-10 08:35:16'),
 (25,6,1,'REOPEN: Problem is not solved
DESCRIPTION: jednak nie działa ;(','2025-05-10 07:34:33'),
 (26,16,1,'Mój służbowy telefon nie chce się połączyć z wifi','2025-05-11 10:00:54.3491195'),
 (27,16,1,'Proszę szybko o naprawę, nie mogę pracować','2025-05-11 10:01:10'),
 (30,14,3,'Zresetowaliśmy dostępy, powinno działac','2025-05-11 10:11:23'),
 (31,17,8,'Wyskakuje błąd podczas zamawiania dostępów do aplikacji xyz','2025-05-11 10:17:10.2164691'),
 (32,17,3,'Naprawilismy bład. Prosze zamowic dostepy.','2025-05-11 10:18:21');
INSERT INTO "Priorities" ("PriorityID","PriorityName") VALUES (1,'Low'),
 (2,'Medium'),
 (3,'High'),
 (4,'Critical');
INSERT INTO "ReopenReasons" ("ReasonID","ReasonText") VALUES (1,'Problem is not solved'),
 (2,'Resolution is not clear'),
 (3,'Additional support needed'),
 (4,'Workaround used, issue not fixed'),
 (5,'Missing information now available');
INSERT INTO "Statuses" ("StatusID","StatusName") VALUES (1,'New'),
 (2,'In progress'),
 (3,'Resolved'),
 (6,'Open');
INSERT INTO "Tickets" ("TicketID","Title","Description","UserID","TechnicianID","StatusID","PriorityID","CreatedDate","ResolvedDate") VALUES (1,'Problem z drukarką','Drukarka nie drukuje, świeci się czerwona dioda.',1,3,6,2,'2025-03-22 15:24:27','2025-04-09 18:24:01.087045'),
 (2,'Nie działa WiFi','Brak połączenia z siecią w biurze.',4,NULL,3,3,'2025-03-22 15:24:27','2025-05-08 13:01:19.2207751'),
 (3,'Aktualizacja systemu','Proszę o zaktualizowanie systemu na moim laptopie.',5,3,3,1,'2025-03-22 15:24:27','2025-05-08 13:09:39.7353296'),
 (4,'Laptop się nie uruchamia','Laptop klienta nie reaguje na włącznik. Może być problem z baterią lub płytą główną.',1,2,2,3,'2025-04-26 07:08:01',NULL),
 (5,'Brak dostępu do sieci','Nie mogę połączyć się z siecią firmową od rana.',3,4,2,2,'2025-04-26 07:08:01',NULL),
 (6,'Problem z kontem pocztowym','Outlook wyrzuca błędy przy wysyłaniu wiadomości.',1,3,6,2,'2025-04-26 07:08:01','2025-05-08 13:09:25.9842532'),
 (7,'Monitor nie działa','Monitor pokazuje tylko czarny ekran, ale komputer działa.',3,3,3,2,'2025-04-26 07:08:01','2025-05-08 13:09:23.1776954'),
 (10,'Zamrożenie aplikacji','Aplikacja CRM zawiesza się przy zapisie danych.',2,4,2,3,'2025-05-02 10:45:00','2025-05-03 14:20:00'),
 (11,'Błąd przy logowaniu','Użytkownik nie może się zalogować mimo poprawnych danych.',3,NULL,1,1,'2025-05-04 09:00:00',NULL),
 (13,'Potrzeba nowego konta','Nowy pracownik potrzebuje konta w systemie ERP.',5,3,3,1,'2025-05-06 07:55:00','2025-05-07 12:00:00'),
 (14,'internet access is not working for me','IWA is not working for me I can only use teams and outlook but no website can be opened by me',1,3,3,3,'2025-05-08 14:00:31.8760049','2025-05-11 10:11:55.7389567'),
 (15,'nie mam dostepu do dokumentów','po urlopie straciła dostep do dokumentów w wikihr.pl',1,3,3,2,'2025-05-08 14:09:57.5668443','2025-05-11 10:08:37.1430502'),
 (16,'Telefon nie łączy się z internetem','Mój służbowy telefon nie chce się połączyć z wifi',1,3,3,3,'2025-05-11 10:00:54.3364613','2025-05-11 10:05:42.7965055'),
 (17,'Nie mogę zamówić dostępów do aplikacji','Wyskakuje błąd podczas zamawiania dostępów do aplikacji xyz',8,3,3,2,'2025-05-11 10:17:10.0824334','2025-05-11 10:18:43.330844');
INSERT INTO "Users" ("UserID","Email","Phone","Role","PasswordHash","FirstName","LastName","IsPasswordChangeRequired","IsInactive") VALUES (1,'user@user.com','111222333','User','user','kamil','kamilowski',0,0),
 (2,'admin@admin.com','444555666','Admin','admin','admin','adminowski',0,0),
 (3,'tech@tech.com','123444123','Technician','technician','tomek','Technician',0,0),
 (4,'jan.kowalski@example.com','111212333','User','user123','grzesiu','braun',0,0),
 (5,'anna.nowak@example.com','444515666','User','user456','anna','kowalska',0,0),
 (6,'support@example.com','777818999','Technician','tech123','support','supportowski',0,0),
 (7,'sadasd@sad.sad','123456789','Technician','1234','sad','sadowski',1,1),
 (8,'kamilcieslak@gmail.com','123455667','User','Kamil123','Kamil','Kamilowski',0,0);
COMMIT;
