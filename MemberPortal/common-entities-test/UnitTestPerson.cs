using Microsoft.VisualStudio.TestTools.UnitTesting;
using StagwellTech.SEIU.CommonEntities;
using System;

namespace common_entities_test
{
    [TestClass]
    public class UnitTestPerson
    {
        private static string PERSON_JSON_SAMPLE = "{\"personId\":1,\"bicCode\":\"bicCode\",\"birthDate\":\"2019-01-01T00:00:00\",\"deathDate\":\"2019-01-01T00:00:00\",\"employmentStatus\":\"employmentStatus\",\"firstName\":\"firstName\",\"insertionDate\":\"2019-01-01T00:00:00\",\"lastName\":\"lastName\",\"middleName\":\"middleName\",\"primaryLanguageCode\":\"primaryLanguageCode\",\"sex\":\"m\",\"sortName\":\"sortName\",\"ssn\":1,\"type\":\"type\",\"suffix\":\"suffix\",\"oipTyoeId\":1,\"updatedDate\":\"2019-01-01T00:00:00\",\"importDate\":\"2019-01-01T00:00:00\",\"dnnUserId\":1,\"hideDependants\":false,\"auth0Sub\":\"auth0Sub\",\"email\":\"email\"}";

        [TestMethod]
        public void TestPersonSerialization()
        {
            var person = new Person();

            person.personId = 1;
            person.bicCode = "bicCode";
            person.birthDate = new DateTime(2019, 01, 01);
            person.deathDate = new DateTime(2019, 01, 01);
            person.employmentStatus = "employmentStatus";
            person.firstName = "firstName";
            person.insertionDate = new DateTime(2019, 01, 01);
            person.lastName = "lastName";
            person.middleName = "middleName";
            person.primaryLanguageCode = "primaryLanguageCode";
            person.sex = 'm';
            person.sortName = "sortName";
            person.ssn = 1;
            person.type = "type";
            person.suffix = "suffix";
            person.oipTyoeId = 1;
            person.updatedDate = new DateTime(2019, 01, 01);
            person.importDate = new DateTime(2019, 01, 01);
            person.dnnUserId = 1;
            person.hideDependants = false;
            person.auth0Sub = "auth0Sub";
            person.email = "email";

            var json = person.toJSON();

            Assert.AreEqual(json, PERSON_JSON_SAMPLE);
        }

        [TestMethod]
        public void TestPersonDeSerialization()
        {
            var person = Person.fromJSON(PERSON_JSON_SAMPLE);

            Assert.AreEqual(person.personId, 1);
            Assert.AreEqual(person.bicCode, "bicCode");
            Assert.AreEqual(person.birthDate, new DateTime(2019, 01, 01));
            Assert.AreEqual(person.deathDate, new DateTime(2019, 01, 01));
            Assert.AreEqual(person.employmentStatus, "employmentStatus");
            Assert.AreEqual(person.firstName, "firstName");
            Assert.AreEqual(person.insertionDate, new DateTime(2019, 01, 01));
            Assert.AreEqual(person.lastName, "lastName");
            Assert.AreEqual(person.middleName, "middleName");
            Assert.AreEqual(person.primaryLanguageCode, "primaryLanguageCode");
            Assert.AreEqual(person.sex, 'm');
            Assert.AreEqual(person.sortName, "sortName");
            Assert.AreEqual(person.ssn, 1);
            Assert.AreEqual(person.type, "type");
            Assert.AreEqual(person.suffix, "suffix");
            Assert.AreEqual(person.oipTyoeId, 1);
            Assert.AreEqual(person.updatedDate, new DateTime(2019, 01, 01));
            Assert.AreEqual(person.importDate, new DateTime(2019, 01, 01));
            Assert.AreEqual(person.dnnUserId, 1);
            Assert.AreEqual(person.hideDependants, false);
            Assert.AreEqual(person.auth0Sub, "auth0Sub");
            Assert.AreEqual(person.email, "email");

        }
    }
}
