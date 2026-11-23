# Diagrama de classes — Negra Visão

Este diagrama representa a estrutura das 11 classes de domínio já declaradas no backend C# do projeto. Foi conferido com `banco de dados/sql_V2.sql`, com o mapeamento de `AppDbContext.cs` e com as regras de negócio de `PI_NegraVisao_v15.docx`.

- [Diagrama em SVG, para ampliar e inserir em documentos](diagrama_classes_negra_visao.svg).
- [Diagrama em PNG](diagrama_classes_negra_visao.png).
- [Fonte editável em Mermaid](diagrama_classes_negra_visao.mmd).

O recorte é o domínio: pessoas, papéis, eventos, inscrições e presenças. Controllers, ViewModels, telas, classes de infraestrutura e o próprio contexto do Entity Framework ficam fora desse recorte.

## Como ler

Cada caixa representa uma classe e lista suas propriedades escalares com os nomes e os tipos usados no C#. `+` indica acesso público; `?` indica que a propriedade aceita nulo; `uint` é um inteiro sem sinal; `DateTime` é o tipo utilizado pelo código, inclusive para datas sem horário no SQL. As propriedades de navegação e as coleções são representadas pelas linhas entre as classes, sem repetição nas caixas.

Os números junto às extremidades das associações são as multiplicidades:

| Notação | Significado |
| --- | --- |
| `1` | Exatamente um objeto |
| `0..1` | Nenhum ou um objeto |
| `0..*` | Nenhum ou vários objetos |
| `1..*` | Pelo menos um objeto; usado nas restrições de negócio abaixo |

Exemplo: `Pessoa "1" -- "0..*" Inscricao` significa que uma pessoa pode ter nenhuma ou várias inscrições, enquanto cada inscrição pertence a exatamente uma pessoa.

As linhas são associações simples. Não foi presumida composição nem exclusão automática dos objetos relacionados: o SQL declara as referências com `ON DELETE NO ACTION`.

## Responsabilidade de cada classe

| Classe | Responsabilidade |
| --- | --- |
| `Pessoa` | Cadastro pessoal único por CPF, compartilhado entre os papéis da pessoa. |
| `Endereco` | Endereço associado a uma pessoa. |
| `Papel` | Identificação dos papéis de participante, responsável, colaborador e administrador. |
| `PessoaPapel` | Vínculo entre uma pessoa e um papel; permite acumular papéis. |
| `UsuarioInterno` | Dados de acesso interno associados a uma pessoa. |
| `Responsavel` | Dados específicos da atuação de uma pessoa como responsável por eventos. |
| `Evento` | Definição da atividade, capacidade, recorrência, inscrições, estado e visibilidade. |
| `EventoResponsavel` | Vínculo entre um evento e um responsável. |
| `Ocorrencia` | Realização concreta de um evento, com início e fim. |
| `Inscricao` | Vínculo de uma pessoa com um evento, com código, estado e faltas consecutivas. |
| `Presenca` | Registro de presença ou falta de uma inscrição em uma ocorrência. |

## Decisões de modelagem

### Papéis são associações

Não há uma hierarquia `Pessoa → Participante / Colaborador / Administrador`. Pela RN40, uma mesma pessoa pode acumular esses papéis sem duplicação do cadastro. A associação `Pessoa → PessoaPapel ← Papel` representa essa regra e corresponde ao código existente.

`Responsavel` e `UsuarioInterno` também estão associados a `Pessoa`; não são subclasses. Os atributos pessoais continuam em `Pessoa`. Possuir cadastro de responsável, isoladamente, não concede acesso interno, conforme a RN38.

### Inscrição pertence ao evento; presença pertence à ocorrência

Uma inscrição cobre o evento e suas ocorrências, conforme a RN11. Por isso, `Inscricao` se associa a `Evento`, enquanto `Presenca` se associa a `Inscricao` e a `Ocorrencia`. Para um evento recorrente, a pessoa se inscreve uma vez e pode receber vários registros de presença ou falta.

### Associações muitos para muitos

Foram mantidas as classes associativas reais do backend:

- `PessoaPapel`: chave composta por `PessoaId` e `PapelId`.
- `EventoResponsavel`: chave composta por `EventoId` e `ResponsavelId`.

`Inscricao` também materializa a relação entre pessoas e eventos, mas possui identidade, código, estado e outros dados próprios. `Presenca` registra a relação entre uma inscrição e uma ocorrência.

## Estrutura persistida e restrições de negócio

O desenho usa as multiplicidades permitidas pela estrutura atual do banco e pelos modelos. Uma chave estrangeira obriga o objeto dependente a ter um pai; ela não obriga todo pai a ter filhos. As regras abaixo refinam essa estrutura para os cadastros válidos do sistema:

| Relação ou propriedade | Estrutura representada | Restrição de negócio |
| --- | --- | --- |
| Pessoa e endereço | Uma pessoa tem `0..1` endereço. | RN23 exige endereço completo no cadastro: para um cadastro válido, deve existir `1`. |
| Evento e ocorrências | Um evento tem `0..*` ocorrências. | RN01 exige exatamente `1` em evento único e pelo menos `2` em evento recorrente. |
| Evento e responsáveis | Um evento tem `0..*` vínculos `EventoResponsavel`. | RN06 exige pelo menos `1` responsável vinculado. |
| Pessoa e papéis | Uma pessoa tem `0..*` vínculos `PessoaPapel`. | RN40 permite acumular papéis; RN35 exige perfil interno de colaborador ou administrador para acesso ao painel. |
| Inscrição e presença | Uma inscrição tem `0..*` registros. | RN46 exige inscrição confirmada que cubra a ocorrência. A ocorrência e a inscrição devem pertencer ao mesmo evento. |

Se a entrega da disciplina exigir um **modelo conceitual de cadastros válidos**, ajuste a extremidade de `Endereco` para `1`, a de `EventoResponsavel` junto ao evento para `1..*` e a de `Ocorrencia` para `1..*`, mantendo a restrição de uma ou pelo menos duas ocorrências conforme a classificação. A versão entregue aqui documenta a estrutura implementada e mantém essas exigências explícitas nesta tabela.

As seguintes unicidades já aparecem no SQL e no `AppDbContext`:

- CPF em `Pessoa`.
- Nome em `Papel`.
- Login em `UsuarioInterno`.
- Código de inscrição em `Inscricao`.
- Combinação de pessoa e evento em `Inscricao`.
- Combinação de inscrição e ocorrência em `Presenca`.
- Pessoa em cada uma das classes `Endereco`, `Responsavel` e `UsuarioInterno`, garantindo os relacionamentos de no máximo um objeto de cada tipo por pessoa.

Os campos de estado e classificação aparecem como `string`, pois esse é o tipo atual das propriedades C#. No SQL, vários desses campos usam `ENUM`. Transformá-los em enumerações C# seria uma alteração de implementação e não foi presumido no diagrama.

## E os métodos?

As 11 classes de domínio atuais possuem propriedades e não declaram operações de negócio. Hoje, parte do comportamento está nos controllers. Por isso, o diagrama não apresenta métodos fictícios como se estivessem implementados nessas entidades. Um diagrama de classes pode documentar somente essa estrutura.

Se a disciplina solicitar atributos **e métodos**, a tabela abaixo é uma proposta de evolução do modelo, sujeita à implementação. São assinaturas conceituais: autorização, persistência e coordenação entre entidades também precisam ser definidas.

| Classe | Operações propostas, ainda não declaradas na entidade | Regras relacionadas |
| --- | --- | --- |
| `Pessoa` | `inativar(): void`, `reativar(): void` | RN20, RN21, RN37 |
| `Evento` | `publicar(): void`, `ocultar(): void`, `cancelar(motivo: string): void` | RN41 a RN44 |
| `Ocorrencia` | `finalizarListaPresenca(): void` | RN47 |
| `Inscricao` | `cancelarAPedido(motivo: string): void`, `reativar(): void`, `encerrarPorFaltas(): void` | RN45, RN49, RN50 |

Essas operações não substituem validações que envolvem vários objetos. Confirmar ou reativar uma inscrição, por exemplo, exige verificar vagas e conflitos com outros eventos; cancelar um evento também afeta ocorrências e inscrições.

## Fontes consultadas

- `backend/NegraVisao_MVC/Models/`: as 11 classes com mapeamento `[Table]`.
- `backend/NegraVisao_MVC/Data/AppDbContext.cs`: relacionamentos e unicidades.
- `backend/NegraVisao_MVC/Controllers/`: localização atual de comportamentos de negócio.
- `banco de dados/sql_V2.sql`: estrutura relacional de referência.
- `banco de dados/seed_dados_iniciais.sql`: nomes dos papéis iniciais.
- `docs/PI_NegraVisao_v15.docx`: regras de negócio.

Observação de rastreabilidade: o README menciona `PI_NegraVisao_v16.docx`, mas esse arquivo não está presente nesta cópia do projeto. Foi consultada a versão 15, disponível no diretório `docs`.
