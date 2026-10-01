# Proposta de reescrita da Etapa 1

Documento-base: `docs/PI_NegraVisao_v3.docx`

Status: proposta consolidada para aprovação. Este texto ainda não foi aplicado ao DOCX.

## Interpretação consolidada do escopo

### Incluído nesta versão

- área pública para consulta dos eventos publicados e acesso aos seus detalhes;
- inscrição de participantes em eventos;
- painel interno para colaboradores e administradores;
- cadastro de eventos e responsáveis pela equipe interna;
- classificação de curso, oficina e palestra como tipos de evento;
- acompanhamento da disponibilidade de vagas;
- validação de presença;
- inativação de participantes pelo administrador;
- gerenciamento de usuários internos pelo administrador;
- consulta e emissão de relatórios administrativos pelo administrador.

### Excluído desta versão

- banco de talentos;
- formulário público para oferecimento de serviços;
- perfis públicos de palestrantes, instrutores ou oficineiros;
- mural de notícias e publicação de artigos;
- portal institucional completo;
- módulos independentes para cursos, oficinas e palestras;
- gestão de parcerias sem relação direta com os eventos.

O contato inicial de uma pessoa interessada em ministrar uma atividade permanece como processo externo. Dentro do sistema, o responsável pelo evento é cadastrado pela equipe interna.

## Objetivo do projeto

Desenvolver uma plataforma web para a Associação Negra Visão que centralize a divulgação de eventos, a inscrição de participantes e as atividades internas de gestão. A solução deverá facilitar o acesso da comunidade a cursos, oficinas e palestras e apoiar a equipe da associação no cadastro de eventos e responsáveis, no acompanhamento das vagas, na validação de presenças e na emissão de relatórios administrativos.

## Solução e produto

A solução será uma plataforma web composta por uma área pública e um painel interno de gestão.

A área pública permitirá que qualquer pessoa consulte os eventos publicados, visualize suas informações e realize uma inscrição. Curso, oficina e palestra serão tratados como tipos de evento, mantendo um fluxo único de consulta e inscrição.

O painel interno será restrito aos colaboradores e administradores da Associação Negra Visão. Os colaboradores poderão cadastrar eventos e responsáveis, acompanhar a disponibilidade de vagas e validar a presença dos participantes. O administrador possuirá todas as permissões do colaborador e também poderá inativar participantes, gerenciar usuários internos e acessar relatórios administrativos.

## Escopo do sistema

Esta versão contempla uma página pública de eventos, o processo de inscrição de participantes e um painel interno de gestão. Na área pública, a comunidade poderá consultar os eventos publicados, visualizar seus detalhes e realizar inscrições. Os eventos poderão ser classificados como curso, oficina ou palestra.

No painel interno, os colaboradores poderão cadastrar eventos e seus responsáveis, acompanhar a disponibilidade de vagas e validar presenças. O administrador terá as mesmas permissões do colaborador e permissões adicionais para inativar participantes, gerenciar usuários internos e acessar relatórios administrativos.

Não fazem parte desta versão o banco de talentos, o cadastro público de pessoas interessadas em ministrar atividades, os perfis públicos de profissionais, o mural de notícias, a publicação de artigos e o portal institucional completo. Os responsáveis pelos eventos serão cadastrados pela equipe interna da associação.

## Stakeholders

### Associação Negra Visão

Instituição responsável pelas decisões de negócio e principal beneficiária da plataforma. Tem interesse na centralização das informações, na organização dos eventos e no acompanhamento das inscrições e presenças.

### Comunidade

Público interessado nos cursos, oficinas e palestras promovidos ou divulgados pela Associação Negra Visão.

### Participantes

Pessoas da comunidade que fornecem seus dados e realizam inscrições nos eventos publicados.

### Responsáveis pelos eventos

Pessoas que ministram ou respondem pela realização dos eventos. Seus dados são cadastrados e vinculados aos eventos pela equipe interna.

### Equipe interna da associação

Grupo formado por colaboradores e administradores que utiliza o painel interno. Os colaboradores executam atividades operacionais, enquanto os administradores possuem permissões adicionais de gestão.

## Atores do sistema

### Participante

Consulta os eventos na área pública, visualiza seus detalhes, fornece seus dados e realiza inscrições.

### Colaborador

Usuário interno que cadastra eventos e responsáveis, acompanha a disponibilidade de vagas e valida presenças.

### Administrador

Especialização funcional do colaborador. Possui as mesmas permissões operacionais e também pode inativar participantes, gerenciar usuários internos e acessar relatórios administrativos.

O responsável pelo evento permanece como stakeholder e entidade do domínio, mas não como ator do sistema enquanto não possuir acesso ou funcionalidade própria na plataforma.

## Glossário

- **Administrador:** usuário interno com as permissões do colaborador e funções adicionais de gestão, inativação de participantes, gerenciamento de usuários e acesso a relatórios administrativos.
- **Área pública:** parte da plataforma destinada à consulta de eventos e à inscrição de participantes.
- **Colaborador:** usuário interno que executa atividades operacionais no painel de gestão.
- **Evento:** atividade disponibilizada pela Associação Negra Visão para consulta e inscrição da comunidade.
- **Inscrição:** registro que vincula um participante a um evento.
- **Painel interno:** área restrita utilizada por colaboradores e administradores.
- **Participante:** pessoa da comunidade que fornece seus dados e se inscreve em eventos. Substitui integralmente o termo “aluno”.
- **Presença:** registro de que o comparecimento do participante foi validado por um usuário interno.
- **Responsável:** pessoa que ministra ou responde pela realização de um evento. Palestrante, instrutor e oficineiro são categorias ou descrições possíveis do responsável.
- **Tipo de evento:** classificação de um evento. Nesta versão, os tipos previstos são curso, oficina e palestra.
- **Usuário interno:** termo coletivo para colaborador e administrador.
- **Vaga:** unidade da capacidade disponível para inscrição em um evento.

## Substituições terminológicas para o restante do documento

| Termo atual | Termo ou tratamento aprovado |
|---|---|
| Aluno | Participante |
| ADM | Administrador |
| Cadastrar aluno | Cadastrar participante |
| Inativar aluno | Inativar participante |
| Palestrante usado como entidade | Responsável |
| Parceiro palestrante | Responsável pelo evento |
| Cursos, eventos e palestras | Eventos dos tipos curso, oficina ou palestra |
| Cadastrar palestrante | Cadastrar responsável |
| Início Institucional | Área pública |
| Plataforma web como stakeholder | Remover |
| Banco de talentos | Remover do escopo desta versão |
| Portal institucional e notícias | Remover do escopo desta versão |

## Impactos que deverão ser tratados nas próximas etapas

- criar requisitos para consulta da lista e dos detalhes dos eventos;
- decidir se colaborador ou administrador pode cadastrar participantes manualmente;
- definir se acompanhamento de vagas é apenas cálculo automático ou inclui alteração manual de capacidade e reservas;
- confirmar que o responsável pelo evento não terá acesso ao sistema nesta versão;
- criar requisitos e casos de uso para autenticação e gerenciamento de usuários internos;
- atualizar requisitos que ainda tratam curso, palestra e evento como conceitos separados;
- atualizar diagramas que ainda usam Aluno, ADM, palestrante e responsável como papéis concorrentes;
- representar o painel interno nos protótipos;
- manter campos cadastrais, confirmação de identidade e envio de e-mail em aberto até a Etapa 2.

## Critérios de aprovação da Etapa 1

- objetivo, solução, escopo e stakeholders descrevem somente as funcionalidades aprovadas;
- Participante substitui completamente Aluno;
- curso, oficina e palestra aparecem apenas como tipos de Evento;
- Responsável, Colaborador e Administrador têm significados distintos;
- stakeholders e atores do sistema estão separados;
- a plataforma não é tratada como stakeholder nem como ator de si própria;
- a área pública e o painel interno possuem limites explícitos;
- nenhuma decisão reservada às etapas de dados, segurança e regras de negócio é apresentada como concluída.
