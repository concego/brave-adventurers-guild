using System;
using System.Collections.Generic;

namespace BraveAdventurersGuild.Logic
{
    public class Adventurer
    {
        // Identidade
        public string Name { get; set; }
        public string CharacterClass { get; set; }
        public int Level { get; set; } = 1;
        public bool IsFounder { get; set; } = false;

        // Atributos Base SRD d20
        public int Strength { get; set; } = 10;
        public int Dexterity { get; set; } = 10;
        public int Constitution { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public int Wisdom { get; set; } = 10;
        public int Charisma { get; set; } = 10;

        // Modificadores (Calculados)
        public int GetModifier(int attributeValue)
        {
            return (int)Math.Floor((attributeValue - 10) / 2.0);
        }

        // Status derivados para o Side-scroller
        public int MaxHP => CalculateMaxHP();
        public int CurrentHP { get; set; }
        
        // Atributos de Classe (Modificadores específicos)
        public int BaseAttackBonus => CalculateBAB();
        public int ArmorClass => 10 + GetModifier(Dexterity);

        public float MovementSpeed => 100.0f + (GetModifier(Dexterity) * 5.0f);
        public float JumpForce => 200.0f + (GetModifier(Dexterity) * 10.0f);

        private int CalculateBAB()
        {
            // Guerreiros têm BAB total, Magos/Ladinos menos (SRD d20)
            if (CharacterClass == "Guerreiro") return Level;
            if (CharacterClass == "Ladino") return (int)(Level * 0.75);
            return (int)(Level * 0.5); // Magos
        }
        
        // Extração
        public float CarryCapacity => Strength * 5.0f; // Exemplo de lógica de carga
        public List<string> Inventory { get; set; } = new List<string>();

        // Taxa de contratação baseada na qualidade (Soma dos modificadores)
        public int HiringFee => CalculateHiringFee();

        private int CalculateHiringFee()
        {
            if (IsFounder) return 0; // Fundador não cobra taxa de si mesmo

            int totalMod = GetModifier(Strength) + GetModifier(Dexterity) + 
                           GetModifier(Constitution) + GetModifier(Intelligence) + 
                           GetModifier(Wisdom) + GetModifier(Charisma);
            
            // Base de 50 moedas + 25 por cada ponto de modificador total positivo
            return 50 + (Math.Max(0, totalMod) * 25);
        }

        private int CalculateMaxHP()
        {
            // Lógica simplificada do d20: Dado de Vida da Classe + Mod CON
            int hitDie = CharacterClass == "Guerreiro" ? 10 : 6;
            return hitDie + GetModifier(Constitution);
        }

        public Adventurer(string name, string characterClass, bool isFounder = false)
        {
            Name = name;
            CharacterClass = characterClass;
            IsFounder = isFounder;
            CurrentHP = MaxHP;
        }
    }
}
