USE FitnessNutritieDB;

CREATE TABLE Supplements (
    SupplementID INT PRIMARY KEY IDENTITY(1,1),  -- Unique identifier for each supplement
    SupplementName NVARCHAR(100) NOT NULL,       -- Name of the supplement
    Description NVARCHAR(MAX),                   -- Detailed description of the supplement
    Category NVARCHAR(50),                       -- Optional category of the supplement
    Dosage NVARCHAR(100),                        -- Recommended dosage
    Benefits NVARCHAR(MAX)                       -- Potential health benefits
);

INSERT INTO Supplements (SupplementName, Description, Category, Dosage, Benefits)
VALUES 
    ('Vitamin D3', 
     'A fat-soluble vitamin that helps the body absorb calcium and phosphorus', 
     'Vitamin', 
     '1000-2000 IU daily', 
     'Supports bone health, immune function, and may reduce depression'),

    ('Omega-3 Fish Oil', 
     'Derived from fish, contains EPA and DHA fatty acids', 
     'Fatty Acids', 
     '1000-2000 mg daily', 
     'Reduces inflammation, supports heart and brain health'),

    ('Magnesium', 
     'A mineral crucial for many bodily functions', 
     'Mineral', 
     '200-400 mg daily', 
     'Supports muscle and nerve function, energy production, and bone health'),

	 ('Vitamin C', 
     'A water-soluble vitamin and powerful antioxidant', 
     'Vitamin', 
     '500-1000 mg daily', 
     'Supports immune system, skin health, and wound healing'),

    ('Zinc', 
     'An essential mineral important for immune function and metabolism', 
     'Mineral', 
     '8-11 mg daily', 
     'Supports immune health, wound healing, and reduces inflammation'),

    ('Probiotics', 
     'Live beneficial bacteria that support gut health', 
     'Digestive Health', 
     '1-10 billion CFU daily', 
     'Improves digestive health, supports immune system, and may reduce inflammation'),

    ('Vitamin B12', 
     'A crucial vitamin for nerve tissue health and red blood cell formation', 
     'Vitamin', 
     '2.4 mcg daily', 
     'Supports energy levels, nervous system, and red blood cell formation'),

    ('Turmeric Curcumin', 
     'A powerful anti-inflammatory and antioxidant compound', 
     'Herbal Supplement', 
     '500-1000 mg daily', 
     'Reduces inflammation, supports joint health, and may have anti-cancer properties'),

    ('Vitamin E', 
     'A fat-soluble antioxidant vitamin', 
     'Vitamin', 
     '15 mg daily', 
     'Supports immune function, prevents oxidative stress, and promotes skin health'),

    ('Iron', 
     'An essential mineral critical for hemoglobin formation', 
     'Mineral', 
     '8-18 mg daily', 
     'Prevents anemia, supports oxygen transport in blood, and maintains energy levels'),

    ('Calcium', 
     'A mineral essential for bone health and muscle function', 
     'Mineral', 
     '1000-1200 mg daily', 
     'Supports bone strength, muscle function, and nerve transmission'),

    ('Vitamin K2', 
     'A fat-soluble vitamin that plays a crucial role in bone and heart health', 
     'Vitamin', 
     '90-120 mcg daily', 
     'Supports bone density, helps prevent calcium buildup in arteries'),

    ('Ashwagandha', 
     'An adaptogenic herb used in traditional Ayurvedic medicine', 
     'Herbal Supplement', 
     '300-500 mg daily', 
     'Reduces stress, anxiety, and may improve brain function and testosterone levels'),
	     ('Whey Protein', 
     'A fast-absorbing protein derived from milk during cheese production', 
     'Protein Supplement', 
     '20-30 g per serving', 
     'Supports muscle growth, aids in muscle recovery, increases protein intake'),

    ('Creatine Monohydrate', 
     'A compound that helps supply energy to muscle cells', 
     'Performance Supplement', 
     '5 g daily', 
     'Increases muscle strength, enhances exercise performance, supports muscle mass gain'),

    ('BCAAs (Branched-Chain Amino Acids)', 
     'Essential amino acids that play a crucial role in muscle protein synthesis', 
     'Amino Acid Supplement', 
     '5-10 g per serving', 
     'Reduces muscle soreness, prevents muscle breakdown, supports muscle recovery'),

    ('Mass Gainer', 
     'High-calorie protein powder designed to help individuals gain weight and muscle mass', 
     'Weight Gain Supplement', 
     '1-2 scoops daily', 
     'Provides high calories and protein, supports muscle growth for those struggling to gain weight'),

    ('Pre-Workout', 
     'A supplement designed to boost energy and performance before exercise', 
     'Performance Supplement', 
     '1 scoop 20-30 minutes before workout', 
     'Increases energy, enhances focus, improves exercise performance and endurance'),

    ('Casein Protein', 
     'A slow-digesting protein that provides a sustained release of amino acids', 
     'Protein Supplement', 
     '20-30 g before bed', 
     'Supports muscle recovery during sleep, prevents muscle breakdown, provides sustained protein release'),

    ('Glutamine', 
     'An amino acid that plays a crucial role in muscle recovery and immune function', 
     'Amino Acid Supplement', 
     '5-10 g daily', 
     'Reduces muscle soreness, supports immune system, aids in muscle recovery'),

    ('Beta-Alanine', 
     'An amino acid that helps improve exercise performance', 
     'Performance Supplement', 
     '2-5 g daily', 
     'Increases muscle endurance, reduces fatigue, improves high-intensity exercise performance'),

    ('L-Carnitine', 
     'An amino acid derivative that plays a role in fat metabolism', 
     'Fat Loss Supplement', 
     '500-2000 mg daily', 
     'Supports fat metabolism, may improve exercise recovery, potentially enhances athletic performance'),

    ('Test Booster', 
     'A supplement designed to naturally increase testosterone levels', 
     'Hormonal Supplement', 
     'As directed on product label', 
     'May support muscle growth, increase strength, improve libido and energy levels');

	 SELECT * FROM Supplements;