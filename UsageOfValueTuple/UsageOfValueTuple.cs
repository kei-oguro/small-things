// This shows what I was misunderstanding about assignment to a ValueTuple.
// I thought that any variable decleared by deconstruction could not be assigned to.
// But actually, the only things that could not be assigned to was a foreach iteration variable.

var (a, b) = (1, 2);
// var (c) = (3); // NG, I was wrong. I thought this declares a single element ValueTuple<int>, but it does not.
var (c, _) = (3, 4);
b = a + c; // OK, can be compiled. I was wrong.
foreach (var (x, y) in new[] { (5, 6), (7, 8) })
{
    b += x + y;
    // x = y; // NG
}