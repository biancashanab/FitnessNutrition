IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FitnessNutritieDB')
BEGIN
    CREATE DATABASE FitnessNutritieDB;
END
GO

USE FitnessNutritieDB;
GO


CREATE TABLE Exercitii (
    ID INT PRIMARY KEY IDENTITY(1,1),
    DenumireExercitiu NVARCHAR(100),
    Repetari INT,
    GrupaMusculara NVARCHAR(100),
    Seturi INT,
    Descriere NVARCHAR(100),
    TimpEstimareExecutie INT
);
GO


CREATE TABLE Retete (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Calorii INT NOT NULL,
    Carbohidrati DECIMAL(5,2) NOT NULL,
    Proteine DECIMAL(5,2) NOT NULL,
    Grasimi DECIMAL(5,2) NOT NULL,
	Nume NVARCHAR(200) NOT NULL,
    Ingrediente NVARCHAR(MAX) NOT NULL,
	TipMasa NVARCHAR(20) NOT NULL CHECK (TipMasa IN ('Mic Dejun', 'Pranz', 'Cina', 'Gustare'))
);
GO

CREATE TABLE Utilizatori (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    HashedPassword NVARCHAR(255) NOT NULL,
    Sex NVARCHAR(16) CHECK (Sex IN ('Masculin', 'Feminin', 'Unspecified')) NULL,
    Height DECIMAL(5,2),
    Kilograms DECIMAL(5,2),
    PhysicalCondition NVARCHAR(50),
    UserType NVARCHAR(20) CHECK (UserType IN ('Utilizator', 'Nutritionist', 'Administrator')) NOT NULL
);

CREATE TABLE IstoricAntrenamente (
    ID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES Utilizatori(ID),
    ExercitiuID INT NOT NULL FOREIGN KEY REFERENCES Exercitii(ID),
    DataExecutie DATE NOT NULL,
    OraExecutie TIME NOT NULL,
);
GO

CREATE TABLE IstoricGreutate (
    ID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES Utilizatori(ID),
    Data DATE NOT NULL,
    Greutate DECIMAL(5,2) NOT NULL
);
GO

CREATE TABLE PlanAlimentarIstoric (
    ID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES Utilizatori(ID),
    RetetaID INT NOT NULL FOREIGN KEY REFERENCES Retete(ID),
    Data DATE NOT NULL,
    TipMasa NVARCHAR(20) NOT NULL CHECK (TipMasa IN ('Mic Dejun', 'Pranz', 'Cina', 'Gustare'))
);
GO

INSERT INTO Exercitii (DenumireExercitiu, Repetari, GrupaMusculara, Seturi, Descriere, TimpEstimareExecutie)
VALUES
('Impins la banca inclinata cu bara', 10, 'push', 4, 'Exercitiu pentru partea superioara a pieptului', 9),
('Flotari cu bratele largi', 12, 'push', 4, 'Exercitiu pentru piept, folosind o priza larga', 7),
('Impins la banca declinata cu bara', 8, 'push', 4, 'Exercitiu pentru partea inferioara a pieptului', 9),
('Flotari pliometrice', 10, 'push', 3, 'Flotari explozive pentru putere si viteza', 6),
('Extensii triceps pe banca cu haltera', 12, 'push', 3, 'Exercitiu pentru izolare triceps', 8),
('Flotari la perete', 15, 'push', 4, 'Exercitiu de baza pentru piept si triceps', 6),
('Impins de la piept cu gantera pe mingea de fitness', 12, 'push', 3, 'Exercitiu pentru stabilitate si piept', 7),
('Flotari in plan inclinat', 10, 'push', 3, 'Exercitiu pentru partea superioara a pieptului', 6),
('Extensii triceps la aparat', 12, 'push', 3, 'Exercitiu pentru triceps cu cablu', 7),
('Presa de umeri la aparat', 8, 'push', 4, 'Exercitiu pentru dezvoltarea umerilor', 9),
('Ramat cu cablu', 12, 'pull', 4, 'Exercitiu pentru dezvoltarea spatelui', 9),
('Tractiuni la inel', 10, 'pull', 3, 'Exercitiu pentru spate folosind inele', 10),
('Ramat la banca inclinata cu gantera', 12, 'pull', 4, 'Exercitiu pentru izolare spate', 8),
('Indreptari sumo', 8, 'pull', 4, 'Varianta sumo a indreptarilor pentru spate si picioare', 12),
('Pulldown la cablu', 12, 'pull', 4, 'Exercitiu pentru spate, simuland tractiuni', 8),
('Tractiuni cu priza ingusta', 10, 'pull', 3, 'Exercitiu pentru spate si biceps', 9),
('Ramat vertical cu bara', 12, 'pull', 3, 'Exercitiu pentru dezvoltarea spatelui superior si trapezului', 7),
('Indreptari cu haltera hexagonala', 8, 'pull', 4, 'Varianta de indreptari cu haltera hexagonala', 11),
('Ramat T-bar', 10, 'pull', 4, 'Exercitiu pentru dezvoltarea spatelui', 9),
('Tractiuni la aparat', 12, 'pull', 3, 'Exercitiu pentru spate cu asistenta', 8),
('Genuflexiuni frontale cu bara', 10, 'legs', 4, 'Exercitiu pentru cvadriceps si abdomen', 10),
('Genuflexiuni sumo', 12, 'legs', 4, 'Exercitiu pentru coapse si fesieri', 9),
('Presa la aparatul Smith', 12, 'legs', 4, 'Exercitiu controlat pentru picioare', 8),
('Indreptari romanesti cu gantere', 10, 'legs', 3, 'Exercitiu pentru hamstrings si fesieri', 10),
('Genuflexiuni cu priza larga', 12, 'legs', 4, 'Exercitiu pentru coapse si fesieri', 9),
('Fandari cu bara', 10, 'legs', 3, 'Exercitiu pentru picioare si fesieri', 8),
('Presa pentru picioare unilaterala', 12, 'legs', 3, 'Exercitiu pentru dezvoltarea fortei pe un picior', 9),
('Salturi pliometrice', 15, 'legs', 3, 'Exercitiu exploziv pentru picioare si rezistenta', 7),
('Ridicari pe varfuri la presa', 15, 'legs', 4, 'Exercitiu pentru gambe la presa pentru picioare', 8),
('Step-up pe cutie cu gantere', 12, 'legs', 3, 'Exercitiu pentru picioare si stabilitate', 9),
('Flotari', 12, 'push', 4, 'Exercitiu pentru piept si triceps', 8),
('Impins la piept cu bara', 8, 'push', 4, 'Exercitiu clasic pentru dezvoltarea pieptului', 10),
('Impins deasupra capului cu gantere', 10, 'push', 3, 'Exercitiu pentru umeri si triceps', 9),
('Flotari la paralele', 12, 'push', 3, 'Exercitiu pentru triceps si piept', 7),
('Tractiuni la bara', 10, 'pull', 3, 'Exercitiu principal pentru spate si biceps', 10),
('Ramat cu gantere', 12, 'pull', 3, 'Exercitiu pentru dezvoltarea spatelui', 9),
('Indreptari cu bara', 8, 'pull', 4, 'Exercitiu pentru spate si hamstrings', 12),
('Flexii pentru biceps cu gantere', 12, 'pull', 3, 'Exercitiu pentru biceps si antebrat', 6),
('Genuflexiuni cu greutati', 15, 'legs', 4, 'Exercitiu complet pentru coapse si fesieri', 10),
('Fandari cu gantere', 12, 'legs', 4, 'Exercitiu pentru picioare si fesieri', 8),
('Presa picioare', 15, 'legs', 4, 'Exercitiu pentru dezvoltarea muschilor picioarelor', 10),
('Indreptari romanesti', 10, 'legs', 3, 'Exercitiu pentru hamstrings si fesieri', 9),
('Curl cu bara', 10, 'biceps', 4, 'Exercitiu pentru dezvoltarea bicepsului brahial', 8),
('Curl cu gantere pe banca inclinate', 12, 'biceps', 3, 'Exercitiu pentru bicepsul brahial', 7),
('Hammer curl', 12, 'biceps', 3, 'Exercitiu pentru biceps si antebrat', 6),
('Concentration curl', 10, 'biceps', 3, 'Exercitiu izolat pentru biceps', 5),
('Extensii pentru triceps cu gantera', 12, 'triceps', 4, 'Exercitiu izolat pentru triceps', 8),
('Flotari la paralele', 10, 'triceps', 4, 'Exercitiu pentru triceps si piept', 7),
('Pushdown la cablu pentru triceps', 12, 'triceps', 3, 'Exercitiu pentru triceps la cablu', 6),
('Extensii cu bara pentru triceps', 8, 'triceps', 4, 'Exercitiu pentru triceps la orizontal', 9),
('Ridicari laterale cu gantere', 12, 'umeri', 4, 'Exercitiu pentru deltoidul lateral', 7),
('Ridicari frontale cu gantere', 10, 'umeri', 3, 'Exercitiu pentru deltoidul anterior', 6),
('Presă militara cu gantere', 8, 'umeri', 4, 'Exercitiu pentru dezvoltarea umerilor', 9),
('Aripioare cu gantere', 12, 'umeri', 3, 'Exercitiu pentru deltoidul posterior', 7),
('Flotari pe o mana', 8, 'piept', 3, 'Exercitiu pentru piept, triceps si stabilitate', 7),
('Impingere cu gantere pe banca inclinata', 10, 'piept', 4, 'Exercitiu pentru piept superior', 8),
('Flotari la aparat', 12, 'piept', 3, 'Exercitiu pentru piept cu suport', 6),
('Dips pentru piept', 10, 'piept', 3, 'Exercitiu pentru piept si triceps', 9),
('Ramat cu bara', 10, 'spate', 4, 'Exercitiu pentru dezvoltarea spatelui', 9),
('Deadlift', 8, 'spate', 4, 'Exercitiu compus pentru intregul spate', 12),
('Pull-up cu priza larga', 8, 'spate', 3, 'Exercitiu pentru spate si biceps', 10),
('Ramat la aparat', 12, 'spate', 3, 'Exercitiu pentru spate cu suport', 8),
('Abdomene', 15, 'abdomen', 4, 'Exercitiu clasic pentru abdomen', 6),
('Plank', 30, 'abdomen', 4, 'Exercitiu pentru stabilitatea abdomenului', 10),
('Crunch-uri inversate', 15, 'abdomen', 3, 'Exercitiu pentru partea inferioara a abdomenului', 5),
('Rotatii rusești', 20, 'abdomen', 4, 'Exercitiu pentru oblici', 8),
('Genuflexiuni cu bara', 12, 'cvadriceps', 4, 'Exercitiu principal pentru cvadriceps', 10),
('Presa picioare', 15, 'cvadriceps', 4, 'Exercitiu pentru dezvoltarea cvadricepsului', 9),
('Fandari inainte', 12, 'cvadriceps', 3, 'Exercitiu pentru picioare si fesieri', 8),
('Genuflexiuni bulgare', 10, 'cvadriceps', 3, 'Exercitiu pentru un picior pentru cvadriceps', 9),
('Indreptari romane', 10, 'biceps femural', 4, 'Exercitiu pentru biceps femural si fesieri', 12),
('Flexii la aparat', 12, 'biceps femural', 3, 'Exercitiu izolat pentru biceps femural', 9),
('Deadlift cu picioarele drepte', 8, 'biceps femural', 4, 'Exercitiu pentru intregul posterior al piciorului', 11),
('Fandari inversate', 10, 'biceps femural', 3, 'Exercitiu pentru biceps femural si fesieri', 8),
('Extensii pentru picioare', 12, 'coapse', 4, 'Exercitiu izolat pentru cvadriceps', 8),
('Fandari laterale', 10, 'coapse', 4, 'Exercitiu pentru coapse si fesieri', 9),
('Abduse cu greutate', 12, 'coapse', 3, 'Exercitiu pentru muschii laterali ai coapselor', 7),
('Presa picioare cu un picior', 10, 'coapse', 3, 'Exercitiu pentru dezvoltarea fortei pe un picior', 10),
('Ridicari de umeri cu gantere', 12, 'trapez', 4, 'Exercitiu pentru dezvoltarea trapezului', 8),
('Shrugs cu bara', 10, 'trapez', 4, 'Exercitiu pentru trapez', 9),
('Ramat cu priza ingusta', 10, 'trapez', 3, 'Exercitiu pentru trapez si spate', 11),
('Extensii de spate', 12, 'trapez', 3, 'Exercitiu pentru intarirea spatelui superior', 10),
('Ridicari pe varfuri cu greutate', 15, 'gambe', 4, 'Exercitiu pentru dezvoltarea gambelor', 8),
('Ridicari pe varfuri la aparat', 12, 'gambe', 3, 'Exercitiu pentru gambe efectuat la aparat', 7),
('Ridicari pe varfuri pe un picior', 10, 'gambe', 3, 'Exercitiu izolat pentru gambe pe un picior', 9),
('Fandari cu ridicari pe varfuri', 12, 'gambe', 4, 'Exercitiu pentru gambe si coapse', 10);

GO


INSERT INTO Retete (Calorii, Carbohidrati, Proteine, Grasimi, Nume, Ingrediente, TipMasa)
VALUES
    (350, 50.00, 20.00, 5.00, 'Fulgi de ovaz cu lapte','Fulgi de ovaz (50g), Lapte (200ml), Fructe de padure (100g)', 'Mic Dejun'),
    (400, 60.00, 15.00, 8.00, 'Omletă cu spanac și roșii', 'Omleta (3 oua), Spanac (50g), Rosii (100g)', 'Mic Dejun'),
    (300, 40.00, 25.00, 7.00, 'Iaurt grecesc cu miere și nuci', 'Iaurt grecesc (200g), Miere (10g), Nuci (30g)', 'Mic Dejun'),
    (450, 55.00, 30.00, 10.00, 'Smoothie proteic', 'Banana (3), Lapte (200ml), Pudra proteica (20g)', 'Mic Dejun'),
    (500, 70.00, 20.00, 12.00, 'Toast cu avocado și ou', 'Paine integrala (2 felii), Avocado (100g), Oua (2 buc)', 'Mic Dejun'),
    (400, 50.00, 25.00, 9.00, 'Brioșe de ovăz și fructe', 'Briose de ovaz (2 buc), Fructe (100g)', 'Mic Dejun'),
    (450, 65.00, 10.00, 7.00, 'Cereale integrale cu lapte', 'Cereale integrale (50g), Lapte (200ml)', 'Mic Dejun'),
    (350, 45.00, 20.00, 8.00, 'Toast cu unt de arahide și banane', 'Paine integrala (2 felii), Unt de arahide (10 gm), Banane (1)', 'Mic Dejun'),
    (400, 50.00, 25.00, 9.00, 'Clatite integrale cu sirop', 'Fina integrala (100g), Oua (2), Apa (10ml), Sirop de artar', 'Mic Dejun'),
    (300, 40.00, 15.00, 5.00, 'Batoane de granola', 'Batoane de granola (2 buc)', 'Mic Dejun'),
	(420, 55.00, 25.00, 10.00, 'Briose cu banane', ' Faina(100g), Oua(2 buc), Lapte(50ml), Banane(2 buc)', 'Mic Dejun'),
    (410, 58.00, 26.00, 9.00, 'Clatite de ovaz', 'Ovaz (100g), Oua (2), Apa (10ml) Sirop de arțar', 'Mic Dejun'),
    (440, 60.00, 25.00, 8.00, 'Iaurt cu fructe', 'Fructe proaspete (200g), Iaurt (150g)', 'Mic Dejun'),
    (500, 65.00, 28.00, 12.00, 'Sandwich cu ou fiert', 'Paine integrala(2 felii), Oua (2 ouă), Salata verde', 'Mic Dejun'),
    (390, 40.00, 20.00, 10.00, 'Granola cu Lapte', 'Granola (50g), Lapte (200ml)', 'Mic Dejun'),

	(500, 60.00, 35.00, 15.00, 'Piept de pui cu orez și broccoli', 'Piept de pui (150g), Orez (150g), Broccoli (100g)', 'Pranz'),
    (600, 80.00, 40.00, 20.00, 'Salată cu ton și quinoa', 'Salata (100g), Ton(50g), Rosii(100g), Quinoa (100g), Legume', 'Pranz'),
    (700, 90.00, 50.00, 25.00, 'Paste cu carne și sos de roșii', 'Paste integrale (100g), Sos de rosii (100g), Carne macinata (100g)', 'Pranz'),
    (550, 75.00, 30.00, 12.00, 'Burger din curcan', 'carne curcan(150g), Ceapa(15g), Rosii(20g), Castraveti murati(10g), Chifle integrale (1)', 'Pranz'),
    (650, 85.00, 40.00, 18.00, 'Supă de legume și pâine integrală', 'Legume la alegere: rosii, telina, cartofi, dovlecei(300ml), Paine integrala (50g)', 'Pranz'),
    (500, 70.00, 30.00, 15.00, 'Taco cu carne și salată', 'Carne de porc (150g), Lipie integrala (1 felii), Salata (50g)', 'Pranz'),
    (600, 75.00, 35.00, 25.00, 'Pizza integrală cu salată', 'Aluat (150g), Sos de rosii(30g), Mozzarella Light(50g), Sunca din piept de pui(50g) Salata', 'Pranz'),
    (700, 90.00, 45.00, 20.00, 'Friptură de vită cu piure', 'Carne de vita (150g), Cartofi (150g), Lapte(25ml)', 'Pranz'),
    (600, 70.00, 50.00, 15.00, 'Wrap cu pui și salată', 'Carne de pui (150g), Sos de smantana(15g), Castraveti murati(15g), Lipie integrala(2 felii), Salata verde (100g), Rosii(100g)', 'Pranz'),
    (800, 100.00, 55.00, 25.00, 'Bol de orez cu legume și tofu', 'Orez (100g), Legume:morcov, mazare, ceapa (50g), Tofu (300g)', 'Pranz'),
	(700, 80.00, 45.00, 22.00, 'Lasagna vegetariana', 'Vinete (100g), Dovelcei (100g), Ceapa (30g), Praz (20g), Sos bechamel (200ml)', 'Pranz'),
    (600, 70.00, 40.00, 20.00, 'Risotto cu ciuperci', 'Orez (250g), Ciuperci (60g)', 'Pranz'),
    (500, 65.00, 25.00, 10.00, 'Pasta cu pesto ', 'Pate(200g), Pesto(100g)', 'Pranz'),
    (700, 100.00, 55.00, 20.00, 'Paste cu legume si branza', 'Paste(150g), Legume:Ardei, Rosii, Ceapa(100g), Branza feta(50g)', 'Pranz'),
    (750, 90.00, 40.00, 22.00, 'Pasta cu ton', 'Paste (200g), Ton(100g), Ceapa(20g), Sos de rosii(200ml)', 'Pranz'),

	(400, 45.00, 25.00, 15.00, 'Somon cu legume', 'Somon (100g), Legume (150g)', 'Cina'),
    (600, 80.00, 40.00, 25.00, 'Tocana de legume și pâine integrală', 'Legume: Cartofi, Morcov, Ceapa, (200g), Paine integrala (50g)', 'Cina'),
    (700, 90.00, 50.00, 20.00, 'Pui cu curry și orez', 'Carne de Pui (150g), Curry (5g), Orez (150g)', 'Cina'),
    (500, 60.00, 35.00, 15.00, 'Chili con carne', 'Carne de proc (100g), Fasole rosie (100g)', 'Cina'),
    (550, 75.00, 30.00, 20.00, 'Peste cu quinoa', 'Peste (150g), Quinoa (100g)', 'Cina'),
    (650, 80.00, 40.00, 25.00, 'Paste cu creveti', 'Pasta integrale (100g), Creveti (70g), Sos alb(100g)', 'Cina'),
    (600, 70.00, 50.00, 18.00, 'Salata caldă cu năut', 'Naut (100g), măsline (2), roșii (100g), ceapa (20g), 1 legătură pătrunjel verde', 'Cina'),
    (800, 100.00, 60.00, 30.00, 'Friptură de porc cu cartofi', 'Carne de porc (150g), Cartofi (150g)', 'Cina'),
    (700, 85.00, 50.00, 20.00, 'Cuscus cu legume', 'Cuscus (100g), Morcov(50g), Pastarnac(50g)', 'Cina'),
    (900, 110.00, 65.00, 25.00, 'Pui pe grătar cu salată', 'Piept de Pui (150g), Salata verde (80g), Rosii(80g), Castraveti(80g)', 'Cina'),
    (450, 50.00, 30.00, 20.00, 'Peste la cuptor cu sparanghel', 'Peste (150g), Sparanghel (100g)', 'Cina'),
    (600, 75.00, 40.00, 25.00, 'Friptura de curcan cu legume mexicane', 'Carne de curcan (200g), Legume:Porumb, Pastai, Ardei (150g)', 'Cina'),
    (700, 90.00, 45.00, 30.00, 'Pizza vegetariana', 'Aluat(150g), Sos de rosii(40g), Mozzarella Light(80g), Ardei gras(30g), Rucola(20g), Porumb(20g)', 'Cina'),
    (550, 70.00, 40.00, 20.00, 'Frittata cu legume ', 'Oua(3 buc), Brocolii(70g) Ardei gras(50g), Ceapa(20g), ', 'Cina'),
    (600, 75.00, 40.00, 20.00, 'Pasta cu carne de vita', 'Carne de vita (150g), Paste(100g), Sos de rosii(100g)', 'Cina'),

	(200, 30.00, 10.00, 5.00, 'Măr și iaurt', 'Mar (1), Iaurt (100g)', 'Gustare'),
    (250, 20.00, 15.00, 6.00, 'Batoane de cereale', 'Batoane (2 buc)', 'Gustare'),
    (150, 20.00, 10.00, 3.00, 'Fructe de pădure', 'Fructe de padure (150g)', 'Gustare'),
    (300, 40.00, 15.00, 10.00, 'Migdale', 'Migdale (30g)', 'Gustare'),
    (200, 25.00, 10.00, 8.00, 'Popcorn', 'Popcorn (50g)', 'Gustare'),
    (250, 30.00, 10.00, 5.00, 'Iaurt grecesc cu miere', 'Iaurt grecesc (100g), Miere (10g)', 'Gustare'),
    (180, 20.00, 8.00, 6.00, 'Brânză cottage', 'Branza cottage (100g)', 'Gustare'),
    (220, 25.00, 12.00, 7.00, 'Banana', 'Banana (1)', 'Gustare'),
    (280, 30.00, 10.00, 5.00, 'Nuci', 'Nuci (30g)', 'Gustare'),
    (150, 15.00, 5.00, 2.00, 'Biscuiți integrali', 'Biscuiti integrali (2 buc)', 'Gustare'),
	(180, 25.00, 10.00, 4.00, 'Iaurt cu fructe', 'Iaurt (100g), Fructe (100g)', 'Gustare'),
    (220, 30.00, 12.00, 5.00, 'Hummus cu legume', 'Castravete (100g), Hummus (50g)', 'Gustare'),
    (170, 25.00, 8.00, 4.00, 'Pere', 'Pere (1)', 'Gustare'),
    (160, 15.00, 5.00, 4.00, 'Iaurt cu cereale ', 'Iaurt(150g), Cereale(50)g', 'Gustare'),
    (190, 20.00, 9.00, 5.00, 'Cheesecake', 'Iaurt(100g), Branza(150g), Oua(2 buc), Zahar(50g)', 'Gustare');


GO

SELECT * FROM Utilizatori;
SELECT * FROM Exercitii;
SELECT * FROM Retete;

-- Drop the existing table if it exists
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PlanAlimentarIstoric')
BEGIN
    DROP TABLE PlanAlimentarIstoric;
END

-- Create the table with 3 columns
CREATE TABLE PlanAlimentarIstoric (
    ID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES Utilizatori(ID),
    RetetaID INT NOT NULL FOREIGN KEY REFERENCES Retete(ID)
);
GO

-- Example insert statements
INSERT INTO PlanAlimentarIstoric (UserID, RetetaID)
VALUES 
(1, 1),  -- User 1 has Reteta 1
(1, 2),  -- User 1 has Reteta 2
(2, 3),  -- User 2 has Reteta 3
(2, 4);  -- User 2 has Reteta 4
GO

-- Query to get retete for a specific user
SELECT R.* 
FROM PlanAlimentarIstoric PAI
JOIN Retete R ON PAI.RetetaID = R.ID
WHERE PAI.UserID = 2;