#region Данные

PetRepository repository = new PetRepository();
repository.Add(new Dog(3, 80.5, "Carly"));
repository.Add(new Dog(7, 105.7, "Mukhtar"));
repository.Add(new Horse("Atlantia", 4, 1005, "Storm"));
repository.Add(new Horse("Giant", 4, 1005, "Light"));
repository.Add(new Cat(6, 99.43, "Latia"));
#endregion

#region Приложение

var app = WebApplication.CreateBuilder().Build();
app.Run(async (context) =>
{
    var request = context.Request;
    var response = context.Response;
    if (request.Path == "/" && request.Method == "GET")
    {
        await response.WriteAsJsonAsync(repository.ReadAll());
    }
    if (request.Path == "/horse" && request.Method == "GET")
    {
        await response.WriteAsJsonAsync(repository.HorseFilter());
    }
    if (request.Path == "/cat" && request.Method == "GET")
    {
        await response.WriteAsJsonAsync(repository.CatFilter());
    }
    if (request.Path == "/dog" && request.Method == "GET")
    {
        await response.WriteAsJsonAsync(repository.DogFilter());
    }
    if (request.Path.Value.Contains("/name/") && request.Method == "GET")
    {
        string name = request.Path.Value.Split('/')[2];
        var result = repository.PetSortFilter(name);
        await response.WriteAsJsonAsync(result);
    }
    if (request.Path.Value.Contains("/id/") && request.Method == "GET")
    {
        string strID = request.Path.Value.Split('/')[2];
        var result = repository.Read(new Guid(strID));
        await response.WriteAsJsonAsync(result);
    }
    if (request.Path == "/dog" && request.Method == "POST")
    {
        Dog_Cat_DTO dogDTO = await request.ReadFromJsonAsync<Dog_Cat_DTO>();
        Dog dog = new Dog(dogDTO.Age, dogDTO.Cost, dogDTO.Name);
        repository.Add(dog);
    }
    if (request.Path == "/cat" && request.Method == "POST")
    {
        Dog_Cat_DTO catDTO = await request.ReadFromJsonAsync<Dog_Cat_DTO>();
        Cat cat = new Cat(catDTO.Age, catDTO.Cost, catDTO.Name);
        repository.Add(cat);
    }
    if (request.Path == "/horse" && request.Method == "POST")
    {
        HorseDTO horseDTO = await request.ReadFromJsonAsync<HorseDTO>();
        Horse horse = new Horse(horseDTO.Breed, horseDTO.Age, horseDTO.Cost, horseDTO.Name);
        repository.Add(horse);
    }
    if (request.Path.Value.Contains("/id/") && request.Method == "DELETE")
    {
        string strID = request.Path.Value.Split('/')[2];
        repository.Delete(new Guid(strID));
    }
});
app.Run();
#endregion


#region Типы и абстракции
class PetRepository
{
    private List<Pet> pets;

    public PetRepository()
    {
        pets = new List<Pet>();
    }
    #region Фильтры
    public List<Pet> groundPetFilter()
    {
        List<Pet> result = new List<Pet>();
        foreach (Pet ground_pet in pets)
        {
            if (ground_pet is Pet pet)
            {
                result.Add(pet);
            }

        }
        return result;
    }
    public List<Horse> HorseFilter()
    {
        List<Horse> result = new List<Horse>();
        foreach (Pet horse_pet in pets)
        {
            if (horse_pet is Horse pet)
            {
                result.Add(pet);
            }

        }
        return result;
    }
    public List<Cat> CatFilter()
    {
        List<Cat> result = new List<Cat>();
        foreach (Pet horse_pet in pets)
        {
            if (horse_pet is Cat pet)
            {
                result.Add(pet);
            }

        }
        return result;
    }
    public List<Dog> DogFilter()
    {
        List<Dog> result = new List<Dog>();
        foreach (Pet horse_pet in pets)
        {
            if (horse_pet is Dog pet)
            {
                result.Add(pet);
            }

        }
        return result;
    }
    public List<Pet> PetSortFilter(string value)
    {
        return pets.FindAll(pets => pets.Name.ToLower().Contains(value.ToLower()));
    }
    #endregion
    #region CRUD
    public void Add(object pet)
    {
        if (pet is not Pet)
        {
            throw new Exception("Некорректный тип данных");
        }
        pets.Add((Pet)pet);
    }

    public List<Pet> ReadAll()
    {
        return pets;
    }

    public Pet Read(Guid id)
    {
        return pets.Find(pet => pet.Id == id);
    }

    public void Delete(Guid id)
    {
        pets.Remove(Read(id));
    }

    public static void ReadList(PetRepository repository)
    {
        foreach (Pet pet in repository.ReadAll())
        {
            Console.WriteLine(pet);
        };
    }
    #endregion
}
class AddChoice
{
    public static void DogChoice(PetRepository repository)
    {
        Console.WriteLine("Введите возраст собаки:");
        int age = Convert.ToInt32(Console.ReadLine());
        CodeExceptions.AgeCheck(age);

        Console.WriteLine("Введите цену за собаки:");
        double cost = Convert.ToDouble(Console.ReadLine());
        CodeExceptions.Positive_Check(cost);

        Console.WriteLine("Введите имя собаки:");
        string name = Console.ReadLine();
        CodeExceptions.String_check(name);

        Dog dog = new Dog(age, cost, name);
        repository.Add(dog);
        Console.WriteLine("Собака успешно добавлена в каталог");
    }

    public static void CatChoice(PetRepository repository)
    {
        Console.WriteLine("Введите возраст кошки:");
        int age = Convert.ToInt32(Console.ReadLine());
        CodeExceptions.AgeCheck(age);

        Console.WriteLine("Введите цену за кошку:");
        double cost = Convert.ToDouble(Console.ReadLine());
        CodeExceptions.Positive_Check(cost);

        Console.WriteLine("Введите имя кошки:");
        string name = Console.ReadLine();
        CodeExceptions.String_check(name);

        Cat cat = new Cat(age, cost, name);
        repository.Add(cat);
        Console.WriteLine("Кошка успешно добавлена в каталог");
    }

    public static void HorseChoice(PetRepository repository)
    {
        Console.WriteLine("Введите породу лошади:");
        string breed = Console.ReadLine();
        CodeExceptions.String_check(breed);

        Console.WriteLine("Введите возраст лошади:");
        int age = Convert.ToInt32(Console.ReadLine());
        CodeExceptions.AgeCheck(age);

        Console.WriteLine("Введите цену за лошадь:");
        double cost = Convert.ToDouble(Console.ReadLine());
        CodeExceptions.Positive_Check(cost);

        Console.WriteLine("Введите имя лошади:");
        string name = Console.ReadLine();
        CodeExceptions.String_check(name);

        Horse horse = new Horse(breed, age, cost, name);
        repository.Add(horse);
        Console.WriteLine("Лошадь успешно добавлена в каталог");
    }
}

public class CodeExceptions
{
    public static void String_check(string s)
    {
        foreach (char letter in s)
        {
            if (!(char.IsLetter(letter)) && letter != ' ')
            {
                throw new Exception("Вы ввели неверное имя");
            }
        }
    }
    public static void Positive_Check(double s)
    {
        if (s < 0)
        {
            throw new Exception("Вы ввели неверную цену");
        }
    }
    public static void AgeCheck(int n)
    {
        if (n < 0 || n > 20)
        {
            throw new Exception("Вы ввели неверный возраст животного");
        }
    }
}

class Pet
{
    private string name;
    private int age;
    private double cost;

    public Guid Id { get; set; }
    public Pet(int age, double cost, string name)
    {
        Age = age;
        Cost = cost;
        Name = name;
        Id = Guid.NewGuid();
    }

    public int Age
    {
        get
        {
            return age;
        }
        set
        {
            CodeExceptions.AgeCheck(value);
            age = value;
        }
    }

    public double Cost
    {
        get => cost;
        set
        {
            CodeExceptions.Positive_Check(value);
            cost = value;
        }
    }

    public string Name
    {
        get => name;
        set
        {
            CodeExceptions.String_check(value);
            name = value;
        }
    }
    public override string ToString()
    {
        return $"Имя - {Name}\n" +
                $"Возраст - {Age}\n" +
                $"Стоимость - {Cost}\n" +
                $"ID - {Id}\n";
    }
}
class Dog_Cat_DTO
{
    public int Age { get; set; }
    public double Cost { get; set; }
    public string Name { get; set; }
}
class HorseDTO
{
    public string Breed { get; set; }
    public int Age { get; set; }
    public double Cost { get; set; }
    public string Name { get; set; }
}
#endregion
#region Наземные
class Dog : Pet
{
    public Dog(int age, double cost, string name) : base(age, cost, name) { }

    public override string ToString()
    {
        return $"Собака:\n" + base.ToString();

    }
}
class Cat : Pet
{
    public Cat(int age, double cost, string name) : base(age, cost, name) { }
    public override string ToString()
    {
        return $"Кошка:\n" + base.ToString();
    }
}
class Horse : Pet
{
    private string breed;
    public string Breed
    {
        get => breed;
        set
        {
            CodeExceptions.String_check(value);
            breed = value;
        }
    }

    public Horse(string breed, int age, double cost, string name) : base(age, cost, name)
    {
        Breed = breed;
    }

    public override string ToString()
    {
        return $"Лошадь:\n" +
                $"Порода: {Breed}\n" +
                base.ToString()
                ;
    }
}
#endregion