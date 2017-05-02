using System;

namespace NearestSmaller {
    public class NearestSmallerTree<TKey, TValue> where TKey : IComparable {
        private NearestSmallerNode<TKey, TValue> root;

        public TValue GetValue(TKey key) {
            var node = root;
            var parent = node;
            while (node != null) {
                if (key.CompareTo(node.Key) < 0) {
                    if (node.Left == null) {
                        return key.CompareTo(parent.Key) > 0 ? parent.Value : default(TValue);
                    }
                        parent = node;
                        node = node.Left;

                }
                else if (key.CompareTo(node.Key) > 0) {
                    if (node.Right == null) {
                        return node.Value;
                    }
                    parent = node;
                    node = node.Right;
                }
                else {
                    break;
                }
            }
            return default(TValue);
        }

        public void Insert(TKey key, TValue value) {
            root = Insert(key, value, root);
        }

        private NearestSmallerNode<TKey, TValue> Insert(TKey key, TValue value, NearestSmallerNode<TKey, TValue> node) {
            if (node == null) {
                return new NearestSmallerNode<TKey, TValue>(key, value);
            }
            if (key.CompareTo(node.Key) < 0) {
                node.Left = Insert(key, value, node.Left);
                if (node.GetBalance() == 2) {
                    node = key.CompareTo(node.Left.Key) < 0 ? RotateRight(node) : RotateLeftRight(node);
                }
            }
            else if (key.CompareTo(node.Key) > 0) {
                node.Right = Insert(key, value, node.Right);
                if (node.GetBalance() == -2) {
                    node = key.CompareTo(node.Right.Key) > 0 ? RotateLeft(node) : RotateRightLeft(node);
                }
            }
            else {
                node.Value = value;
            }
            node.RecalculateHeight();
            return node;
        }

        private NearestSmallerNode<TKey, TValue> RotateLeft(NearestSmallerNode<TKey, TValue> node1) {
            var node2 = node1.Right;

            node1.Right = node2.Left;
            node2.Left = node1;

            node1.RecalculateHeight();
            node2.RecalculateHeight();

            return node2;
        }

        private NearestSmallerNode<TKey, TValue> RotateRight(NearestSmallerNode<TKey, TValue> node1) {
            var node2 = node1.Left;

            node1.Left = node2.Right;
            node2.Right = node1;

            node1.RecalculateHeight();
            node2.RecalculateHeight();

            return node2;
        }

        private NearestSmallerNode<TKey, TValue> RotateLeftRight(NearestSmallerNode<TKey, TValue> node1) {
            node1.Left = RotateLeft(node1.Left);
            return RotateRight(node1);
        }

        private NearestSmallerNode<TKey, TValue> RotateRightLeft(NearestSmallerNode<TKey, TValue> node1) {
            node1.Right = RotateRight(node1.Right);
            return RotateLeft(node1);
        }


        private class NearestSmallerNode<TNKey, TNValue> {
            private int Height { get; set; }
            internal TNKey Key { get; }
            internal TNValue Value { get; set; }
            internal NearestSmallerNode<TNKey, TNValue> Left { get; set; }
            internal NearestSmallerNode<TNKey, TNValue> Right { get; set; }

            internal NearestSmallerNode(TNKey key, TNValue value) {
                Key = key;
                Value = value;
            }

            internal void RecalculateHeight() {
                Height = Math.Max(GetLeftHeight(), GetRightHeight()) + 1;
            }

            internal int GetBalance() {
                return GetLeftHeight() - GetRightHeight();
            }

            private int GetLeftHeight() {
                return Left?.Height ?? -1;
            }

            private int GetRightHeight() {
                return Right?.Height ?? -1;
            }
        }
    }
}