using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Library;
using Library.Exceptions;

namespace Library.Tests
{
    public class CharacterTests
    {
        [Fact]
        public void Character_TakeDamage_HealthDecreases()
        {
            var character = new Isaac();
            int initialHealth = character.CurrentHealth;
            int damage = 10;

            character.TakeDamage(damage);

            Assert.Equal(initialHealth - damage, character.CurrentHealth);
        }

        [Fact]
        public void Character_Heal_HealthIncreases()
        {
            var character = new Isaac();
            character.TakeDamage(30);
            int damagedHealth = character.CurrentHealth;
            int healAmount = 20;

            character.Heal(healAmount);

            Assert.Equal(damagedHealth + healAmount, character.CurrentHealth);
        }

        [Fact]
        public void Character_IsAlive_ReturnsTrueWhenHealthAboveZero()
        {
            var character = new Isaac();

            var isAlive = character.IsAlive();

            Assert.True(isAlive);
        }

        [Fact]
        public void Character_IsAlive_ReturnsFalseWhenHealthZeroOrBelow()
        {
            var character = new Isaac();
            character.TakeDamage(character.CurrentHealth);

            var isAlive = character.IsAlive();

            Assert.False(isAlive);
        }

    }
}
