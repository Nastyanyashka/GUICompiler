# Реализация метода рекурсивного спуска для синтаксического анализа.


## Грамматика
```
G[Program]:
1. Program -> ε | Instr Program
2. Instr -> '+' | '-' | '>' | '<' | ',' | '.' | '[' Program ']'
```
Грамматика является Контекстно свободной

## Тестовые примеры
![alt text](ex1.png)
![alt text](ex2.png)
![alt text](ex3.png)
