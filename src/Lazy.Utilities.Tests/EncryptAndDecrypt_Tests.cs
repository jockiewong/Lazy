using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shouldly;
using Lazy.Utilities.EncryptDecrypt;
namespace Lazy.Utilities.Tests
{
    public class EncryptAndDecrypt_Tests
    {
        [Theory]
        [InlineData("123", "test1hrth", "wleghweigb")]
        [InlineData("wselghwopehgopwe4ghtop", "wefgwegf", "erger6345345")]
        [InlineData("2945623h&*,<123815724", "12312wert", "erer34525*")]
        [InlineData(";erhme'rpiy", "wegwedfghrty")]
        public void Tests(string input, string key, string iv = null)
        {
            if (iv != null)
            {
                input.Des(key, iv).UnDes(key, iv).ShouldBe(input);
                input.AesStr(key, iv).UnAesStr(key, iv).ShouldBe(input);
            }
            else
            {
                input.Des(key).UnDes(key).ShouldBe(input);
                input.AesStr(key).UnAesStr(key).ShouldBe(input);
            }
            input.Base64().UnBase64().ShouldBe(input);
        }
    }
}
