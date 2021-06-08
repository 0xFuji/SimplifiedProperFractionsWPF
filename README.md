# SimplifiedProperFractionsWPF
A visual basic WPF app that counts and displays all simplified proper fractions under a specified denominator.

# Algorithm
```py
def commonFactors(i,j):
    for a in range(2, i):
        if i%a==0 and  j%a==0:
            return True
    return False

def simplifiedProperFractions(n):
    total = 0
    for denom in range(2, n):
        for numer in range(1, denom):
            if not commonFactors(denom, numer):
                print(f"{numer}/{denom}")
                total += 1
    return total
```
