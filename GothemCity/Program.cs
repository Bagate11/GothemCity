namespace GothemCity
{
 
    public class Sak
    {
        public string Name { get; set; }
        public Sak(string name)
        {
            Name = name;
        }

    }
    public abstract class Person
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int XVägg { get; set; }
        public int YVägg { get; set; }

        public List<Sak> Inventory { get; set; }
        protected static Random random = new Random();

        public Person(int x, int y)
        {
            X = x;
            Y = y;
            Inventory = new List<Sak>();
            RandomDirection();

        }

        public void RandomDirection()
        {
            var directions = new (int, int)[]
            {
                (-1, 0),
                (1, 0),
                (0, -1),
                (0, 1),
                (-1, 1),
                (1, -1),

            };

            var dir = directions[random.Next(directions.Length)];
            XVägg = dir.Item1;
            YVägg = dir.Item2;
        }

        public void Move()
        {
            X += XVägg;
            Y += YVägg;

            if (X < 0) X = 99;
            if (X > 99) X = 0;
            if (Y < 0) Y = 24;
            if (Y > 24) Y = 0;
        }
   
        

        public abstract void Kontakt(Person other);

        public abstract char Symbol();

    }
    public class Medborgare : Person
    {
        public Medborgare(int x, int y) : base(x, y)
        {
            Inventory.Add(new Sak("Nycklar"));
            Inventory.Add(new Sak("Telefon"));
            Inventory.Add(new Sak("Pengar"));
            Inventory.Add(new Sak("ROLEX"));
        }

        public override void Kontakt(Person other)
        {
            if (other is Tjuv tjuv)
            {
                if (Inventory.Count > 0)
                {
                    var stulen = Inventory[random.Next(Inventory.Count)];
                    Inventory.Remove(stulen);
                    tjuv.Inventory.Add(stulen);
                    Console.WriteLine($"Tjuven rånade Medborgaren och tog {stulen.Name}.");
                    Thread.Sleep(2000);

                }
            }


            else if (other is Polis)
            {
                Console.WriteLine("Medboragren Mötte en Polis, Dom hälsade på varandra");
                Thread.Sleep(2000);


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
                polis.Inventory.AddRange(Inventory);
                Inventory.Clear();
                Console.WriteLine("Polisen har grippit Tjuven och tog allt vad Tjuven hade stulit!");
                Thread.Sleep(2000);



            }

        }

        public override char Symbol()
        {
            return 'T';
        }


    }

        public class Polis : Person
        {
            public Polis(int x, int y) : base(x, y) { }

            public override void Kontakt(Person other)
            {
                if (other is Tjuv tjuv)
                {
                    Inventory.AddRange(tjuv.Inventory);
                    tjuv.Inventory.Clear();
                    Console.WriteLine("Polisen tog tjuvrn och beslagtog Tjuvens stulna grejer");
                    Thread.Sleep(2000);

                }
            }

            public override char Symbol()
            {
                return 'P';
            }
        }

        class Stad
        {
            static void Main(string[] args)
            {
                List<Person> personer = new List<Person>();
                Random random = new Random();
                int AntalRånadeMedborgare = 0;
                int AntalGripnaTjvar = 0;
                int StadBreed = 100;
                int StadHöjd = 25;

                for (int i = 0; i < 50; i ++)
                    personer.Add(new Medborgare(random.Next(StadBreed), random.Next(StadHöjd)));

                for (int h = 0; h < 20; h ++)
                    personer.Add(new Tjuv(random.Next(StadBreed), random.Next(StadHöjd)));
                for (int j = 0; j < 15;  j ++)
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

                                if (personer[i] is Tjuv && personer[j] is Medborgare)
                                {
                                    AntalRånadeMedborgare++;
                                }

                                else if (personer[i] is Polis && personer[j] is Tjuv)
                                {
                                    AntalGripnaTjvar++;
                                }

                                Thread.Sleep(2000);
                            }
                        }
                    }

                    Console.Clear();
                    Console.WriteLine($"Antal rånade medborgare {AntalRånadeMedborgare}   ");
                    Console.WriteLine($"antal gripna tjuvar {AntalGripnaTjvar}");

                    char[,] stad = new char[StadBreed, StadHöjd];
                    for (int y = 0; y < StadHöjd; y++)
                    {
                        for (int x = 0; x < StadBreed; x ++)
                        {
                        Console.Write (stad[x, y] = '.' );
                        }
                    Console.WriteLine();
                    }
                    foreach (var person in personer)
                    {
                        stad[person.X, person.Y] = person.Symbol();
                    }
                   
                
                
                for (int y = 0; y < StadHöjd; y++)
                    {
                        for (int x = 0; x < StadBreed; x++)
                        {
                            Console.Write(stad[x, y ]);
                        }
                        Console.WriteLine();
                    }

                    Thread.Sleep(500);
                }
            }
        }
    }


                    


                
            

            
    
    


            



        
                    



    

        


    
// test 