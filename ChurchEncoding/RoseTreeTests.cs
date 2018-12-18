using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploeh.Samples.ChurchEncoding
{
    public class RoseTreeTests
    {
        [Fact]
        public void MatchLeaf()
        {
            IRoseTree<string, int> tree = new RoseLeaf<string, int>(42);
            int actual = tree.Match(new MatchIntLeafParameters());
            Assert.Equal(42, actual);
        }

        private class MatchIntLeafParameters :
            IRoseTreeParameters<string, int, int>
        {
            public int RunLeaf(int leaf)
            {
                return leaf;
            }

            public int RunNode(
                string node,
                IEnumerable<IRoseTree<string, int>> branches)
            {
                return -1;
            }
        }

        [Fact]
        public void MatchNode()
        {
            IRoseTree<string, int> tree =
                RoseTree.Node("foo",
                    new RoseLeaf<string, int>(42),
                    new RoseLeaf<string, int>(1337));
            int actual = tree.Match(new MatchStringNodeParameters());
            Assert.Equal(3, actual);
        }

        private class MatchStringNodeParameters :
            IRoseTreeParameters<string, int, int>
        {
            public int RunLeaf(int leaf)
            {
                return leaf;
            }

            public int RunNode(
                string node,
                IEnumerable<IRoseTree<string, int>> branches)
            {
                return node.Length;
            }
        }

        [Fact]
        public void LeafIsLeaf()
        {
            var sut = new RoseLeaf<bool, double>(2);
            var actual = sut.IsLeaf();
            Assert.True(actual.ToBool());
        }

        [Fact]
        public void LeafIsNotNode()
        {
            var sut = new RoseLeaf<bool, double>(-4);
            var actual = sut.IsNode();
            Assert.False(actual.ToBool());
        }

        [Fact]
        public void NodeIsNode()
        {
            var sut =
                RoseTree.Node<Guid, Version>(
                    new Guid(0x90B5DF4F, 0xE996, 0x416E, 0xB6, 0x77, 0xEF, 0x4F, 0x38, 0x5E, 0x03, 0xC3));
            var actual = sut.IsNode();
            Assert.True(actual.ToBool());
        }

        [Fact]
        public void NodeIsNotLeaf()
        {
            var sut = RoseTree.Node<Version, Guid>(new Version(2, 0));
            var actual = sut.IsLeaf();
            Assert.False(actual.ToBool());
        }

        [Fact]
        public void CataLeaf()
        {
            IRoseTree<string, int> tree = new RoseLeaf<string, int>(42);
            int actual = tree.Cata((x, xs) => x.Length + xs.Sum(), x => x);
            Assert.Equal(42, actual);
        }

        [Fact]
        public void CataNode()
        {
            IRoseTree<string, int> tree =
                RoseTree.Node(
                    "foo",
                    new RoseLeaf<string, int>(42),
                    new RoseLeaf<string, int>(1337));
            int actual = tree.Cata((x, xs) => x.Length + xs.Sum(), x => x);
            Assert.Equal(1382, actual);
        }

        public class Unit
        {
            public readonly static Unit Instance = new Unit();

            private Unit() { }
        }

        [Fact]
        public void UseMeertensTree()
        {
            IRoseTree<Unit, int> meertensTree =
                RoseTree.Node(Unit.Instance,
                    RoseTree.Node(Unit.Instance,
                        RoseTree.Node(Unit.Instance,
                            new RoseLeaf<Unit, int>(2112)),
                        new RoseLeaf<Unit, int>(42),
                        new RoseLeaf<Unit, int>(1337),
                        new RoseLeaf<Unit, int>(90125)),
                    RoseTree.Node(Unit.Instance,
                        new RoseLeaf<Unit, int>(1984)),
                    new RoseLeaf<Unit, int>(666));
            Assert.False(meertensTree.IsLeaf().ToBool());
        }

        private static readonly IRoseTree<int, string> exampleTree =
            RoseTree.Node(42,
                RoseTree.Node(1337,
                    new RoseLeaf<int, string>("foo"),
                    new RoseLeaf<int, string>("bar")),
                RoseTree.Node(2112,
                    RoseTree.Node(90125,
                        new RoseLeaf<int, string>("baz"),
                        new RoseLeaf<int, string>("qux"),
                        new RoseLeaf<int, string>("quux")),
                    new RoseLeaf<int, string>("quuz")),
                new RoseLeaf<int, string>("corge"));

        [Fact]
        public void SumExample()
        {
            int actual =
                exampleTree.Cata((x, xs) => x + xs.Sum(), x => x.Length);
            Assert.Equal(93641, actual);
        }

        [Fact]
        public void LeafCountExample()
        {
            int actual =
                exampleTree.Cata((_, xs) => xs.Sum(), _ => 1);
            Assert.Equal(7, actual);
        }

        [Fact]
        public void MaximumDepthExample()
        {
            int actual =
                exampleTree.Cata((_, xs) => 1 + xs.Max(), _ => 0);
            Assert.Equal(3, actual);
        }

        private static T Id<T>(T x) => x;

        public static IEnumerable<object[]> BifunctorLawsData
        {
            get
            {
                yield return new[] { new RoseLeaf<int, string>("") };
                yield return new[] { new RoseLeaf<int, string>("foo") };
                yield return new[] { RoseTree.Node<int, string>(42) };
                yield return new[] { RoseTree.Node(42, new RoseLeaf<int, string>("bar")) };
                yield return new[] { exampleTree };
            }
        }

        [Theory, MemberData(nameof(BifunctorLawsData))]
        public void SelectNodeObeysFirstFunctorLaw(IRoseTree<int, string> t)
        {
            Assert.Equal(t, t.SelectNode(Id));
        }

        [Theory, MemberData(nameof(BifunctorLawsData))]
        public void SelectLeafObeysFirstFunctorLaw(IRoseTree<int, string> t)
        {
            Assert.Equal(t, t.SelectLeaf(Id));
        }

        [Theory, MemberData(nameof(BifunctorLawsData))]
        public void SelectBothObeysIdentityLaw(IRoseTree<int, string> t)
        {
            Assert.Equal(t, t.SelectBoth(Id, Id));
        }

        [Theory, MemberData(nameof(BifunctorLawsData))]
        public void ConsistencyLawHolds(IRoseTree<int, string> t)
        {
            DateTime f(int i) => new DateTime(i);
            bool g(string s) => string.IsNullOrWhiteSpace(s);

            Assert.Equal(t.SelectBoth(f, g), t.SelectLeaf(g).SelectNode(f));
            Assert.Equal(
                t.SelectNode(f).SelectLeaf(g),
                t.SelectLeaf(g).SelectNode(f));
        }

        [Theory, MemberData(nameof(BifunctorLawsData))]
        public void SecondFunctorLawHoldsForSelectNode(IRoseTree<int, string> t)
        {
            char f(bool b) => b ? 'T' : 'F';
            bool g(int i) => i % 2 == 0;

            Assert.Equal(
                t.SelectNode(x => f(g(x))),
                t.SelectNode(g).SelectNode(f));
        }

        [Theory, MemberData(nameof(BifunctorLawsData))]
        public void SecondFunctorLawHoldsForSelectLeaf(IRoseTree<int, string> t)
        {
            bool f(int x) => x % 2 == 0;
            int g(string s) => s.Length;

            Assert.Equal(
                t.SelectLeaf(x => f(g(x))),
                t.SelectLeaf(g).SelectLeaf(f));
        }

        [Theory, MemberData(nameof(BifunctorLawsData))]
        public void SelectBothCompositionLawHolds(IRoseTree<int, string> t)
        {
            char f(bool b) => b ? 'T' : 'F';
            bool g(int x) => x % 2 == 0;
            bool h(int x) => x % 2 == 0;
            int i(string s) => s.Length;

            Assert.Equal(
                t.SelectBoth(x => f(g(x)), y => h(i(y))),
                t.SelectBoth(g, i).SelectBoth(f, h));
        }

        [Fact]
        public void MenuExample()
        {
            IRoseTree<string, string> editMenuTemplate =
                RoseTree.Node("Edit",
                    RoseTree.Node("Find and Replace",
                        new RoseLeaf<string, string>("Find"),
                        new RoseLeaf<string, string>("Replace")),
                    RoseTree.Node("Case",
                        new RoseLeaf<string, string>("Upper"),
                        new RoseLeaf<string, string>("Lower")),
                    new RoseLeaf<string, string>("Cut"),
                    new RoseLeaf<string, string>("Copy"),
                    new RoseLeaf<string, string>("Paste"));

            var commandStore = new CommandStore();
            IRoseTree<string, Command> editMenu =
                from name in editMenuTemplate
                select commandStore.Lookup(name);

            var roundTripped = from command in editMenu
                               select command.Name;
            Assert.Equal(editMenuTemplate, roundTripped);
        }

        private class CommandStore
        {
            private readonly List<Command> commands;

            public CommandStore()
            {
                commands = new List<Command>
                {
                    new FindCommand("Find"),
                    new ReplaceCommand("Replace"),
                    new UpperCaseCommand("Upper"),
                    new LowerCaseCommand("Lower"),
                    new CutCommand("Cut"),
                    new CopyCommand("Copy"),
                    new PasteCommand("Paste")
                };
            }

            public Command Lookup(string name)
            {
                return commands
                    .DefaultIfEmpty(new Command(name))
                    .FirstOrDefault(c => c.Name == name);
            }

        }

        public class Command
        {
            public Command(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public virtual void Execute()
            {
            }
        }

        public class FindCommand : Command
        {
            public FindCommand(string name) : base(name)
            {
            }
        }

        public class ReplaceCommand : Command
        {
            public ReplaceCommand(string name) : base(name)
            {
            }
        }

        public class UpperCaseCommand : Command
        {
            public UpperCaseCommand(string name) : base(name)
            {
            }
        }

        public class LowerCaseCommand : Command
        {
            public LowerCaseCommand(string name) : base(name)
            {
            }
        }

        public class CutCommand : Command
        {
            public CutCommand(string name) : base(name)
            {
            }
        }

        public class CopyCommand : Command
        {
            public CopyCommand(string name) : base(name)
            {
            }
        }

        public class PasteCommand : Command
        {
            public PasteCommand(string name) : base(name)
            {
            }
        }
    }

}
