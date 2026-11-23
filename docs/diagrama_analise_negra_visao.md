# Diagrama de classes de análise - Negra Visão

Modelo de análise apresentado no estilo do exemplo de sala de aula: oito classes, atributos com tipos e visibilidade e relacionamentos com multiplicidades. Não contém métodos. A disposição foi organizada manualmente no SVG; a fonte Mermaid preserva o mesmo conteúdo, mas pode produzir outra disposição ao ser renderizada.

- Pessoa: cadastro de quem participa ou atua na ONG. Pode acumular os papéis de participante, responsável, colaborador e administrador.
- Usuário interno: credenciais e situação de acesso, associado a uma pessoa. Seu perfil distingue colaborador de administrador.
- Responsável: descrição da atuação de uma pessoa como responsável por eventos.
- Evento: atividade oferecida pela ONG.
- Inscrição: participação de uma pessoa em um evento.
- Ocorrência: encontro de um evento em uma data e horário.
- Presença: registro de presença ou falta de uma inscrição em uma ocorrência.
- Relatório: documento gerado pelo administrador, com tipo, período, data de geração e formato. É um conceito da análise, não uma nova tabela.

Os atributos são conceituais. `endereco` resume o endereço completo da pessoa. Os tipos `String`, `int`, `Date`, `DateTime` e `boolean` comunicam a natureza dos dados; não prescrevem a linguagem de implementação. O sinal `-` indica um atributo privado. IDs e chaves estrangeiras foram omitidos, pois as ligações já expressam as relações entre os objetos. Nem todo atributo precisa estar preenchido em todas as situações; por exemplo, frequência se aplica a eventos recorrentes.

Pessoa se associa a no máximo um cadastro de responsável e a no máximo um usuário interno. Os dados pessoais não são duplicados nessas classes. Cada evento tem pelo menos um responsável, e um responsável pode atuar em nenhum ou vários eventos. A mesma pessoa pode se inscrever em eventos, possuir acesso interno e atuar como responsável.

Uma inscrição pertence ao evento e cobre seus encontros. A presença é registrada para cada ocorrência. No modelo de análise, um evento válido possui uma ou várias ocorrências: exatamente uma quando único e pelo menos duas quando recorrente. Cada registro de presença deve ligar uma inscrição e uma ocorrência do mesmo evento, sem duplicidade do par.

As multiplicidades são `1` (exatamente um), `0..*` (nenhum ou vários) e `1..*` (um ou vários). As linhas são associações simples.

Este recorte abrange pessoas, acesso interno, responsáveis, eventos, inscrições, presenças e relatórios. A seta tracejada entre usuário interno e relatório indica dependência de geração, restrita ao administrador; não cria um vínculo persistente no banco. Não há métodos, classes associativas técnicas ou herança entre perfis. A referência de aula orienta a apresentação, enquanto o domínio segue as regras do projeto Negra Visão.

Arquivos:

- [Diagrama em SVG](diagrama_analise_negra_visao.svg)
- [Diagrama em PNG](diagrama_analise_negra_visao.png)
- [Fonte Mermaid](diagrama_analise_negra_visao.mmd)
- [PDF com o diagrama de análise e o MER](../output/pdf/Negra_Visao_Diagrama_de_Analise_e_MER.pdf)

Referências: regras de negócio da documentação do projeto (RN01, RN06, RN11, RN40 e RN46) e o modelo de dados existente. O MER incluído no PDF foi mantido como referência da implementação do banco.
