# ValidaCPF

App feito em **.NET MAUI** pra validar CPF. Talvez não esteja estruturado da melhor forma, mas fiz esse projeto pra treinar lógica, manipulação de string/array em C# e um pouco de XAML — nada muito sério, mas ficou funcional e com uma interface decente.

## O que ele faz

- Você digita o CPF (com máscara automática `000.000.000-00` enquanto digita, usando `CommunityToolkit.Maui`)
- Valida se o CPF é matematicamente válido
- Mostra o cálculo dos dois dígitos verificadores
- Se o CPF for válido, mostra a região fiscal onde ele foi originalmente emitido

## Como funciona a validação de um CPF

Um CPF tem 11 dígitos: **9 dígitos base** + **2 dígitos verificadores**. Os dígitos verificadores não são aleatórios — eles são calculados matematicamente a partir dos 9 primeiros, o que é justamente o que permite validar um CPF sem precisar consultar nenhuma base de dados.

### Cálculo do 10º dígito (penúltimo)

1. Multiplica cada um dos 9 primeiros dígitos por um peso decrescente, de 10 até 2.
2. Soma todos os resultados.
3. Pega o resto da divisão dessa soma por 11.
4. Se o resto for menor que 2, o dígito é `0`. Senão, o dígito é `11 - resto`.

### Cálculo do 11º dígito (último)

Mesma lógica, só que agora usando os **10 primeiros dígitos** (os 9 originais + o dígito calculado no passo anterior), com pesos de 11 até 2.

Se os dois dígitos calculados baterem com os dois últimos dígitos do CPF digitado, o CPF é válido.

### Por que isso não prova que o CPF existe de verdade

Esse cálculo garante que o número é **estruturalmente válido** — ou seja, segue a regra matemática da Receita Federal. Mas não garante que aquele número foi realmente emitido pra alguém. Por isso o app também bloqueia sequências tipo `111.111.111-11`, que passam no cálculo mas nunca são emitidas de verdade (são bloqueadas manualmente, verificando se todos os dígitos são iguais).

### A curiosidade da região fiscal

O **9º dígito** do CPF (o último antes dos verificadores) indica a região fiscal onde o CPF foi emitido pela primeira vez:

| Dígito | Região |
|:---:|---|
| 0 | RS |
| 1 | DF, GO, MS, MT, TO |
| 2 | AC, AM, AP, PA, RO, RR |
| 3 | CE, MA, PI |
| 4 | AL, PB, PE, RN |
| 5 | BA, SE |
| 6 | MG |
| 7 | ES, RJ |
| 8 | SP |
| 9 | PR, SC |

O app só mostra essa informação quando o CPF é válido — mostrar a região de um número inventado não faz sentido.

## Tecnologias usadas

- .NET 8 / .NET MAUI
- CommunityToolkit.Maui (máscara de texto do campo de CPF)
- C# puro para toda a lógica de validação (sem bibliotecas externas de validação)

## Rodando o projeto

```bash
git clone <url-do-repo>
cd ValidaCPF
dotnet restore
dotnet build
```

Abra no Visual Studio ou rode direto pelo `dotnet run` selecionando o target framework desejado (ex: `net8.0-windows10.0.19041.0`).

## Possíveis melhorias futuras

- Validar CPFs de teste conhecidos (ex: `123.456.789-09`)
- Indicar qual dos dois dígitos verificadores falhou, em vez de só "inválido"
- Testes unitários pro algoritmo de validação
