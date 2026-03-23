#include <stdio.h>
#include <stdlib.h>
#include <string.h>

extern char* symbolic_demangle_symbol(const char* input);
extern void symbolic_demangle_free(char* s);

static int failures = 0;

static void test(const char* input, const char* expected)
{
    char* result = symbolic_demangle_symbol(input);
    if (expected == NULL) {
        if (result != NULL) {
            fprintf(stderr, "FAIL: \"%s\" expected NULL, got \"%s\"\n", input, result);
            symbolic_demangle_free(result);
            failures++;
        }
    } else if (result == NULL) {
        fprintf(stderr, "FAIL: \"%s\" expected \"%s\", got NULL\n", input, expected);
        failures++;
    } else if (strcmp(result, expected) != 0) {
        fprintf(stderr, "FAIL: \"%s\" expected \"%s\", got \"%s\"\n", input, expected, result);
        symbolic_demangle_free(result);
        failures++;
    }
    if (result != NULL)
        symbolic_demangle_free(result);
}

int main(void)
{
    test("_ZN3foo3barEv", "foo::bar()");
    test("_ZN4main10main_innerEv", "main::main_inner()");
    test("?foo@@YAHXZ", "int foo(void)");
    test("main", NULL);
    test("", NULL);

    if (failures > 0) {
        fprintf(stderr, "%d test(s) failed\n", failures);
        return 1;
    }
    printf("All tests passed\n");
    return 0;
}
