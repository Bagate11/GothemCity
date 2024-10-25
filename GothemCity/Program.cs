namespace GothemCity
{
    public class Things // ändrade Classen namn från sak tiil Things
    {
        public string Name { get; set; }
        public Things(string name) // ändrade till Things 
        {
            Name = name;
        }
    }

    public abstract class Person
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int DirectionX { get; set; }
        public int DirectionY { get; set; }

        public List<Things> Lagra { get; set; } // ändrade här till Things
        protected static Random random = new Random();
        public bool Fängslad { get; set; }
        public Person(int x, int y)
        {
            X = x;
            Y = y;
            Lagra = new List<Things>(); // ändrade det här också till Things 
            Fängslad = false;
            RandomDirection();
        }

        public void RandomDirection()
        {
            var directions = new (int, int)[]
            {
                (-1, 0), (1, 0), (0, -1), (0, 1), (-1, 1), (1, -1),
            };

            var dir = directions[random.Next(directions.Length)];
            DirectionX = dir.Item1;
            DirectionY = dir.Item2;
        }

        public virtual void Move()
        {
            if (!Fängslad)
            {
                X += DirectionX;
                Y += DirectionY;

                if (X < 0) X = 99;
                if (X > 99) X = 0;
                if (Y < 0) Y = 24;
                if (Y > 24) Y = 0;
            }
        }

        public abstract void Kontakt(Person other);
        public abstract char Symbol();
    }

    public class Medborgare : Person
    {
        public Medborgare(int x, int y) : base(x, y)
        {
            Lagra.Add(new Things("Nycklar")); // ändrade New Ska till new Things 
            Lagra.Add(new Things("Telefon"));
            Lagra.Add(new Things("Pengar"));
            Lagra.Add(new Things("ROLEX"));
        }

        public override void Kontakt(Person other)
        {
            if (other is Tjuv tjuv)
            {
                if (Lagra.Count > 0)
                {
                    var stulen = Lagra[random.Next(Lagra.Count)];
                    Lagra.Remove(stulen);
                    tjuv.Lagra.Add(stulen);
                    Console.WriteLine($"Tjuven rånade Medborgaren och tog {stulen.Name}.");
                    Thread.Sleep(1000);
                }
            }
            else if (other is Polis)
            {
                Console.WriteLine("Medboragren mötte en Polis, de hälsade på varandra.");
                Thread.Sleep(1000);
            }
        }

        public override char Symbol()
        {
            return 'M';
        }
    }

    public class Tjuv : Person
    {
        public Tjuv(int x, int y) : base(x, y) { }

        public override void Kontakt(Person other)
        {
            if (other is Polis polis)
            {
                polis.Lagra.AddRange(Lagra);
                Lagra.Clear();
                Console.WriteLine("Polisen har gripit Tjuven och tagit allt stöldgods!");
                Thread.Sleep(1000); // ändrade Hastigheten 
                Fängelse();
            }
        }

        public void Fängelse()
        {
            
            X = Stad.FängelseXStart + Stad.random.Next(Stad.FängelseXSlut - Stad.FängelseXStart);
            Y = Stad.FängelseYStart + Stad.random.Next(Stad.FängelseYSlut - Stad.FängelseYStart);
            Fängslad = true;
            Console.WriteLine($"Tjuven har blivit satt i fängelset på ({X}, {Y}).");
        }

        public override void Move()
        {
            if (Fängslad)
            {
                
                X += DirectionX;
                Y += DirectionY;

                if (X < Stad.FängelseXStart) X = Stad.FängelseXSlut - 1;
                if (X >= Stad.FängelseXSlut) X = Stad.FängelseXStart;
                if (Y < Stad.FängelseYStart) Y = Stad.FängelseYSlut - 1;
                if (Y >= Stad.FängelseYSlut) Y = Stad.FängelseYStart;

                Console.WriteLine($"Tjuven är i fängelset och rör sig till ({X}, {Y}).");
            }
            else
            {
                base.Move(); 
            }
        }

        public override char Symbol()
        {
            return Fängslad ? 'F' : 'T';
        }
    }

    public class Polis : Person
    {
        public Polis(int x, int y) : base(x, y) { }

        public override void Kontakt(Person other)
        {
            if (other is Tjuv tjuv && !tjuv.Fängslad)
            {
                Lagra.AddRange(tjuv.Lagra);
                tjuv.Lagra.Clear();
                Console.WriteLine("Polisen tog Tjuven och beslagtog alla stulna föremål.");
                Thread.Sleep(2000);
                tjuv.Fängelse();
            }
        }

        public override char Symbol()
        {
            return 'P';
        }
    }

    class Stad
    {
        public static int FängelseXStart = 100;
        public static int FängelseXSlut= 110;
        public static int FängelseYStart = 0;
        public static int FängelseYSlut = 5;

        public static Random random = new Random();

        static void Main(string[] args)
        {
            List<Person> personer = new List<Person>();
            Random random = new Random();

            int StadBreed = 100;
            int StadHöjd = 25;

            
            for (int i = 0; i < 30; i++)
                personer.Add(new Medborgare(random.Next(StadBreed), random.Next(StadHöjd)));
            for (int h = 0; h < 20; h++)
                personer.Add(new Tjuv(random.Next(StadBreed), random.Next(StadHöjd)));
            for (int i = 0; i < 10; i++)
                personer.Add(new Polis(random.Next(StadBreed), random.Next(StadHöjd)));

            while (true)
            {
                foreach (var person in personer)
                {
                    person.Move();
                }

                for (int i = 0; i < personer.Count; i++)
                {
                    for (int j = i + 1; j < personer.Count; j++)
                    {
                        if (personer[i].X == personer[j].X && personer[i].Y == personer[j].Y)
                        {
                            personer[i].Kontakt(personer[j]);
                            personer[j].Kontakt(personer[i]);
                            Thread.Sleep(2000);
                        }
                    }
                }

                Console.Clear();
                char[,] stad = new char[StadBreed + 10, StadHöjd];

                for (int y = 0; y < StadHöjd; y++)
                {
                    for (int x = 0; x < StadBreed; x++)
                    {
                        stad[x, y] = '.';
                    }
                }

                for (int y = Stad.FängelseYStart; y < Stad.FängelseYSlut; y++)
                {
                    for (int x = Stad.FängelseXStart; x < Stad.FängelseXSlut; x++)
                    {
                        stad[x, y] = '#';
                    }
                }

                foreach (var person in personer)
                {
                    stad[person.X, person.Y] = person.Symbol();
                }

                for (int y = 0; y < StadHöjd; y++)
                {
                    for (int x = 0; x < StadBreed + 10; x++)
                    {
                        Console.Write(stad[x, y]);
                    }
                    Console.WriteLine();
                }

                Thread.Sleep(500);
            }
        }
    }
}

                
            

            
    
    


            



        
                    



    

        


    
// test 
