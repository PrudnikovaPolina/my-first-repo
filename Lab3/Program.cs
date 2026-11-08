namespace Laba3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // тест Singleton
            Console.WriteLine("Проверка Singleton:");
            GameConfig config1 = GameConfig.GetInstance();
            GameConfig config2 = GameConfig.GetInstance();

            Console.WriteLine($"Текущий уровень сложности в config1: {config1.Difficulty}");
            config2.Difficulty = "Hard"; // меняем данные через вторую переменную
            Console.WriteLine($"Сложность в config1 после изменения через config2: {config1.Difficulty}");
            Console.WriteLine($"Обе переменные ведут на один объект? {object.ReferenceEquals(config1, config2)}\n");


            // тест Abstract Factory и Builder
            Console.WriteLine("Проверка Abstract Factory и Builder:");

            // задаем эльфийский стиль снаряжения с помощью фабрики
            IEquipmentFactory elvenStyle = new ElvenEquipmentFactory();
            ICharacterBuilder builder = new StyledCharacterBuilder(elvenStyle);

            // строитель пошагово собирает экипировку персонажа
            builder.BuildWeapon();
            builder.BuildArmor("Воин света");
            builder.BuildAmulet();

            FullCharacter elvenHero = builder.GetResult();
            string heroDescription = elvenHero.GetFullEquipment();

            Console.WriteLine("Строитель успешно собрал экипировку персонажа:");
            Console.WriteLine(heroDescription);
            Console.WriteLine();


            // тест Factory Method
            Console.WriteLine("Проверка Factory Method:");

            // нанимаем героя ближнего боя и отправляем в бой в созданном снаряжении
            HeroGuild meleeGuild = new MeleeHeroGuild();
            meleeGuild.DeployHero(heroDescription);

            Console.WriteLine();

            // нанимаем героя дальнего боя с тем же снаряжением
            HeroGuild rangeGuild = new RangeHeroGuild();
            rangeGuild.DeployHero(heroDescription);

            Console.ReadLine();
        }
    }

    //SINGLETON
    public class GameConfig
    {
        private static GameConfig _instance;

        public string Difficulty { get; set; }
        public int MaxPlayers { get; set; }

        private GameConfig()
        {
            Difficulty = "Normal";
            MaxPlayers = 4;
        }

        public static GameConfig GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameConfig();
            }
            return _instance;
        }
    }

    //FACTORY METHOD
    public interface IHero
    {
        void Attack(string equipment);
    }

    public class MeleeHero : IHero
    {
        public void Attack(string equipment)
        {
            var config = GameConfig.GetInstance();
            Console.WriteLine($"Воин ближнего боя (сложность: {config.Difficulty}): идет на таран. Экипировка: {equipment}");
        }
    }

    public class RangeHero : IHero
    {
        public void Attack(string equipment)
        {
            var config = GameConfig.GetInstance();
            Console.WriteLine($"Лучник дальнего боя (сложность: {config.Difficulty}): стреляет издалека. Экипировка: {equipment}");
        }
    }

    public abstract class HeroGuild
    {
        public abstract IHero CreateHero();

        public void DeployHero(string equipment)
        {
            IHero hero = CreateHero();
            hero.Attack(equipment);
        }
    }

    public class MeleeHeroGuild : HeroGuild
    {
        public override IHero CreateHero() => new MeleeHero();
    }

    public class RangeHeroGuild : HeroGuild
    {
        public override IHero CreateHero() => new RangeHero();
    }

    //ABSTRACT FACTORY (комплекты брони и оружия)
    public interface IWeapon { string GetName(); }
    public interface IAmulet { string GetName(); }

    //семейство 1 - эльфийское снаряжение
    public class ElvenWeapon : IWeapon { public string GetName() => "Лук звездного света"; }
    public class ElvenAmulet : IAmulet { public string GetName() => "Амулет листвы"; }

    //семейство 2 — орочье снаряжение
    public class OrcWeapon : IWeapon { public string GetName() => "Ржавый топор"; }
    public class OrcAmulet : IAmulet { public string GetName() => "Ожерелье из клыков"; }

    public interface IEquipmentFactory
    {
        IWeapon CreateWeapon();
        IAmulet CreateAmulet();
    }

    public class ElvenEquipmentFactory : IEquipmentFactory
    {
        public IWeapon CreateWeapon() => new ElvenWeapon();
        public IAmulet CreateAmulet() => new ElvenAmulet();
    }

    public class OrcEquipmentFactory : IEquipmentFactory
    {
        public IWeapon CreateWeapon() => new OrcWeapon();
        public IAmulet CreateAmulet() => new OrcAmulet();
    }

    //BUILDER (сборка персонажа)
    public class FullCharacter
    {
        public string Weapon { get; set; }
        public string Armor { get; set; }
        public string Amulet { get; set; }

        public string GetFullEquipment()
        {
            return $"Оружие: {Weapon}, тело: {Armor}, аксессуар: {Amulet}";
        }
    }

    public interface ICharacterBuilder
    {
        void BuildWeapon();
        void BuildArmor(string armorName);
        void BuildAmulet();
        FullCharacter GetResult();
    }

    public class StyledCharacterBuilder : ICharacterBuilder
    {
        private FullCharacter _character = new FullCharacter();
        private IEquipmentFactory _equipmentFactory;

        public StyledCharacterBuilder(IEquipmentFactory equipmentFactory)
        {
            _equipmentFactory = equipmentFactory;
        }

        public void BuildWeapon() => _character.Weapon = _equipmentFactory.CreateWeapon().GetName();
        public void BuildArmor(string armorName) => _character.Armor = armorName;
        public void BuildAmulet() => _character.Amulet = _equipmentFactory.CreateAmulet().GetName();

        public FullCharacter GetResult() => _character;
    }
}
