using System;
using System.Runtime.InteropServices.ComTypes;
using API.Models.Data.Query;
using API.Process;
using Xunit;

namespace UnitTest
{
    public class AgendaTest
    {
        private Agenda _agenda;

        public AgendaTest()
        {
            var jsonEditor = new JsonEditorFake();
            var  dbAgenda = new DbAgendaFake();
            _agenda = new Agenda(dbAgenda, jsonEditor);
        }
        
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
