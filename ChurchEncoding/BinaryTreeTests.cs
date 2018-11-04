using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploeh.Samples.ChurchEncoding
{
    public class BinaryTreeTests
    {
        [Fact]
        public void BasicBinaryTreeExample()
        {
            var source =
                BinaryTree.Create(
                    BinaryTree.Create(
                        BinaryTree.Leaf(0),
                        1337,
                        BinaryTree.Leaf(-22)),
                    42,
                    BinaryTree.Leaf(100));

            IBinaryTree<string> dest = source.Select(i => i.ToString());

            Assert.Equal("42", dest.Match((_l, x, _r) => x, x => x));
        }

        [Fact]
        public void BinaryTreeQuerySyntaxExample()
        {
            var source =
                BinaryTree.Create(
                    BinaryTree.Create(
                        BinaryTree.Leaf(0),
                        1337,
                        BinaryTree.Leaf(-22)),
                    42,
                    BinaryTree.Leaf(100));

            IBinaryTree<string> dest = from i in source
                                       select i.ToString();

            Assert.Equal("42", dest.Match((_l, x, _r) => x, x => x));
        }

        public static IEnumerable<object[]> Trees
        {
            get
            {
                yield return new[] { BinaryTree.Leaf(0) };
                yield return new[] {
                    BinaryTree.Create(
                        BinaryTree.Leaf(2),
                        -3,
                        BinaryTree.Leaf(99)) };
                yield return new[] {
                    BinaryTree.Create(
                        BinaryTree.Create(
                            BinaryTree.Leaf(0),
                            1337,
                            BinaryTree.Leaf(-22)),
                        42,
                        BinaryTree.Leaf(100)) };
                yield return new[] {
                    BinaryTree.Create(
                        BinaryTree.Leaf(2),
                        -927,
                        BinaryTree.Create(
                            BinaryTree.Leaf(88),
                            211,
                            BinaryTree.Leaf(132))) };
                yield return new[] {
                    BinaryTree.Create(
                        BinaryTree.Create(
                            BinaryTree.Leaf(113),
                            -336,
                            BinaryTree.Leaf(-432)),
                        111,
                        BinaryTree.Create(
                            BinaryTree.Leaf(-32),
                            1299,
                            BinaryTree.Leaf(773))) };
            }
        }

        [Theory, MemberData(nameof(Trees))]
        public void FirstFunctorLaw(IBinaryTree<int> tree)
        {
            Assert.Equal(tree, tree.Select(x => x));
        }

        [Theory, MemberData(nameof(Trees))]
        public void SecondFunctorLaw(IBinaryTree<int> tree)
        {
            string g(int i) => i.ToString();
            bool f(string s) => s.Length % 2 == 0;

            Assert.Equal(tree.Select(g).Select(f), tree.Select(i => f(g(i))));
        }
    }
}
