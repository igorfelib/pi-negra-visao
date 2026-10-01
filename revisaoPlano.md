# Plano de revisão da documentação Negra Visão V3

Documento-base: `docs/PI_NegraVisao_v3.docx`

## Objetivo

Revisar a documentação em etapas pequenas e verificáveis, mantendo requisitos, regras de negócio, casos de uso, diagramas, protótipos e modelo de dados coerentes entre si.

O DOCX original deve ser preservado. As alterações futuras devem ser feitas em uma cópia de trabalho e validadas ao final de cada etapa.

## Como usar este plano

- `[ ]` pendente
- `[~]` em andamento
- `[x]` concluído e validado
- `[!]` depende de decisão do grupo ou da instituição

Cada etapa possui um critério de conclusão. A etapa só deve ser marcada como concluída quando esse critério tiver sido atendido.

## Diagnóstico resumido da V3

### Bloqueadores

- [ ] O arquivo é V3, mas o título da primeira página ainda informa **V2**.
- [ ] O RF03 desapareceu; a lista salta de RF02 para RF04, embora a notificação em tela continue presente nos casos de uso, no fluxo e em outros requisitos.
- [ ] RN10 possui dois significados diferentes:
  - na lista principal, RN10 trata da frequência do evento;
  - na documentação do caso de uso, RN10 diz que o participante só pode inscrever o próprio perfil.
- [ ] As seções “Diagrama de classe” e “Modelagem do Banco de Dados” continuam vazias.
- [ ] O sistema precisa enviar e-mail, mas e-mail não faz parte dos dados obrigatórios definidos em RN12 e RF02.
- [ ] O fluxo consulta um CPF e preenche dados pessoais automaticamente, sem autenticação ou verificação de titularidade.

### Problemas importantes

- [ ] “Histórico de Revisão” contém distribuição de tarefas, e não um histórico de versões.
- [ ] “Participante”, “responsável”, “palestrante”, “parceiro”, “colaborador” e “administrador” ainda precisam de definições formais.
- [ ] Cursos, palestras, oficinas e eventos são usados de maneira intercambiável sem um modelo conceitual explícito.
- [ ] Recorrência, frequência, ocorrência e status de inscrição não estão suficientemente definidos.
- [ ] A rastreabilidade entre RN, RF e UC apresenta associações incorretas ou incompletas.
- [ ] O protótipo solicita e-mail, telefone e CEP, mas RN12/RF02 solicitam nome, CPF, data de nascimento e endereço.
- [ ] O diagrama de casos de uso ainda usa “Aluno”, “ADM”, “Inativar aluno” e outros termos da versão anterior.
- [ ] O próprio sistema é indicado como ator secundário na documentação do caso de uso.
- [ ] Não existem requisitos de LGPD, segurança, retenção de dados, auditoria ou controle de exportações.

### Problemas visuais confirmados

- [ ] A tabela introdutória é dividida entre as páginas 1 e 2 e deixa uma grande área vazia no início da página 2.
- [ ] A lista de stakeholders é cortada entre as páginas 2 e 3.
- [ ] O texto do escopo é cortado no meio de uma frase entre as páginas 3 e 4.
- [ ] O diagrama de atividades está pequeno demais para leitura confortável na página 4.
- [ ] O título “Requisitos De Domínio” fica no fim da página 4, separado do restante da seção.
- [ ] Requisitos funcionais são divididos no meio de frases e blocos entre as páginas 5 a 8.
- [ ] A tabela de atores é dividida entre as páginas 8 e 9 sem repetir o cabeçalho.
- [ ] O UC05 é dividido entre as páginas 9 e 10.
- [ ] O título “Protótipo” fica órfão no final da página 10; as imagens começam na página 11.
- [ ] A página 12 contém os títulos dos diagramas técnicos, mas nenhum conteúdo.
- [ ] Na página 13, “linha 7.” e “FLUXO DE EXCEÇÃO” aparecem unidos, sem separação adequada.
- [ ] A página 14 possui grande área vazia após o conteúdo.
- [ ] O Word registra 13 páginas nos metadados, enquanto a renderização atual gera 14 páginas, indicando paginação instável entre renderizadores.

### Estrutura e acessibilidade

- [ ] O título usa estilo Normal, e não o estilo de título.
- [ ] Os 15 títulos usam apenas Heading 1; não existe hierarquia de subtítulos.
- [ ] As seis imagens não possuem texto alternativo.
- [ ] As oito tabelas não possuem linhas de cabeçalho semanticamente marcadas.
- [ ] Há 512 trechos e 53 parágrafos com formatação direta, o que dificulta manter a aparência consistente.
- [ ] A tabela introdutória possui largura fixa de 6,40 polegadas, maior que a área útil de 5,91 polegadas.
- [ ] A tabela de atividades possui largura de 6,17 polegadas, também maior que a área útil.
- [ ] Não há sumário automático, numeração de páginas, legendas ou referências para diagramas e protótipos.

---

# Etapas de revisão

## Etapa 1 — Fixar escopo e vocabulário `[x]`

### Objetivo

Definir o que o sistema realmente entregará e eliminar termos concorrentes antes de reescrever requisitos.

**Status:** concluída. O escopo-base, o vocabulário principal e as três decisões complementares foram aprovados e aplicados em `docs/PI_NegraVisao_v4.docx`. A versão V3 foi preservada.

### Proposta-base aprovada

- Produto focado em página pública de eventos, inscrição de participantes e painel interno de gestão.
- Painel interno com cadastro de eventos e responsáveis, controle de vagas, validação de presença e relatórios.
- Curso, oficina e palestra tratados como tipos de evento, e não como entidades independentes.
- `Participante`: pessoa que se cadastra e se inscreve em eventos.
- `Responsável`: pessoa que ministra ou responde por um evento; “palestrante” pode ser uma categoria ou descrição desse responsável.
- `Colaborador`: usuário interno que opera cadastros e valida presenças.
- `Administrador`: usuário interno com todas as permissões do colaborador e permissões adicionais de gestão, inativação e relatórios.
- Banco de talentos, notícias e portal institucional completo ficam fora do escopo desta versão, salvo decisão expressa do grupo.

### Decisões necessárias

- [x] O produto contempla eventos, inscrições e o painel interno; não inclui banco de talentos nesta versão.
- [x] A área pública será uma página de eventos e inscrições; notícias e portal institucional completo ficam fora desta versão.
- [x] Curso, palestra e oficina serão tipos da entidade `Evento`.
- [x] O termo oficial para o usuário inscrito será `Participante`.
- [x] `Responsável` é quem ministra ou responde pelo evento; palestrante, instrutor e oficineiro são categorias ou descrições do responsável. `Colaborador` é um usuário interno da associação.
- [x] `Administrador` é uma especialização funcional do colaborador, com permissões adicionais de gestão, inativação de participantes, gerenciamento de usuários e relatórios.
- [x] O participante realiza o próprio cadastro e a própria inscrição; usuários internos não cadastram participantes manualmente.
- [x] Usuários internos autorizados podem ajustar a capacidade do evento; o sistema calcula automaticamente as vagas disponíveis com base na capacidade e nas inscrições válidas.
- [x] O responsável não possui acesso ao sistema nesta versão; seus dados são cadastrados e mantidos pela equipe interna.

### Alterações previstas

- [x] Criar um glossário curto com os termos aprovados.
- [x] Reescrever objetivo, solução e escopo usando apenas o vocabulário aprovado.
- [x] Remover promessas que estejam fora do escopo ou criar requisitos correspondentes.
- [x] Retirar “a plataforma WEB” da lista de stakeholders; o sistema não é stakeholder.
- [x] Separar stakeholders humanos/organizacionais de atores do sistema.

### Critério de conclusão

Objetivo, escopo, stakeholders e glossário descrevem o mesmo produto e não utilizam termos concorrentes sem definição.

## Etapa 2 — Definir dados, identidade, segurança e LGPD `[x]`

### Objetivo

Resolver quais dados são realmente necessários e como o sistema comprovará a identidade do participante.

**Status:** concluída e ajustada. As decisões sobre identificação, campos, permissões, validações, relatórios e limitações da primeira versão foram aplicadas em `docs/PI_NegraVisao_v6.docx`. As versões V4 e V5 foram preservadas. Auditoria, retenção automatizada, exclusão/anonimização e aviso de privacidade permanecem declarados fora do escopo.

### Diagnóstico inicial da V4

- A RN12 exige nome, CPF, data de nascimento e endereço, mas não exige e-mail, embora o sistema envie o código de inscrição por e-mail.
- O protótipo acrescenta telefone e CEP, sem finalidade ou obrigatoriedade coerente com a RN12 e o caso de uso.
- A simples digitação do CPF recupera os demais dados do participante, sem comprovação de identidade e com risco de exposição de dados pessoais.
- O documento não define autenticação do participante, recuperação de acesso, autorização por perfil nem auditoria de consultas e exportações.
- A exportação de relatórios aparece como opcional, mas não possui restrição de finalidade, conteúdo mínimo ou registro de auditoria.
- Não há definição de base legal, aviso de privacidade, retenção, exclusão ou atendimento aos direitos do titular.

### Decisões confirmadas

- O CPF será o identificador utilizado para localizar o participante e facilitar novas inscrições.
- O participante não terá conta, login ou senha nesta versão.
- E-mail e telefone não serão obrigatórios, pois parte do público atendido não possui esses meios de contato.
- Nome, CPF, data de nascimento e endereço serão obrigatórios. O endereço e o CEP possuem finalidade territorial e serão utilizados para produzir relatórios de alcance da comunidade.
- Quando o CPF ainda não estiver cadastrado, o participante preencherá o formulário completo.
- Quando o CPF já estiver cadastrado, o sistema recuperará todos os dados registrados e os exibirá no formulário exclusivamente com máscaras, sem apresentar os valores completos.
- Os campos mascarados serão editáveis no fluxo público. O participante poderá substituir a máscara pelo valor completo atualizado antes de confirmar a inscrição. Campos não alterados preservarão os valores armazenados, e as máscaras nunca serão gravadas como dados reais.
- A primeira versão não terá verificação adicional da titularidade do CPF. O documento deverá declarar que o CPF funciona como identificador de busca, e não como mecanismo de autenticação.
- A primeira versão não terá fluxo digital específico para crianças. Adultos e crianças utilizarão o mesmo processo e estarão sujeitos à mesma exigência de CPF.
- Como consequência do escopo escolhido, crianças sem CPF não poderão ser inscritas pelo fluxo público desta versão; eventual atendimento fora desse fluxo não faz parte do sistema documentado.
- O administrador terá acesso total a todas as funcionalidades do sistema, incluindo todas as funcionalidades atribuídas ao colaborador.
- O colaborador poderá cadastrar e editar eventos, cadastrar e editar responsáveis, visualizar e editar o cadastro completo dos participantes, alterar seus dados pessoais e confirmar presenças.
- Somente o administrador poderá gerar, visualizar e exportar relatórios administrativos ou relatórios com dados de participantes. O colaborador não terá acesso a esses relatórios nem às respectivas exportações.
- O responsável continuará sem acesso ao sistema e aos dados dos participantes.
- O CPF deverá conter 11 dígitos e possuir dígitos verificadores válidos. Um CPF inválido impedirá o cadastro ou a inscrição.
- Um CPF já cadastrado sempre reutilizará o participante existente e nunca criará um segundo cadastro.
- O e-mail poderá permanecer vazio; quando preenchido, deverá possuir formato válido.
- O telefone poderá permanecer vazio; quando preenchido, deverá conter DDD e uma quantidade válida de dígitos.
- O CEP deverá possuir oito dígitos.
- A data de nascimento não poderá estar no futuro.
- O sistema impedirá o cadastro quando algum campo obrigatório estiver vazio.
- A primeira versão não implementará trilha de auditoria para consultas, alterações ou exportações.
- A primeira versão não implementará política ou rotina automatizada de retenção e descarte de dados.
- A primeira versão não implementará solicitação, exclusão, bloqueio ou anonimização de dados pessoais.
- A primeira versão não apresentará aviso de privacidade no formulário de cadastro ou inscrição.
- Esses quatro itens serão registrados como limitações conhecidas e candidatos a uma versão futura.

### Diretrizes para a reescrita

- Utilizar endereço completo apenas na base interna e apresentar, nos relatórios comuns, localidades agregadas por bairro, comunidade, região ou CEP, sem expor endereços individuais.
- Remover do documento promessas de envio obrigatório por e-mail e qualquer exigência de conta do participante.
- Declarar explicitamente as limitações conhecidas da primeira versão, sem apresentar como implementados os controles excluídos do escopo.

### Decisões necessárias

- [x] Definir os campos obrigatórios do participante: nome, CPF, data de nascimento, endereço e CEP.
- [x] Justificar a necessidade de data de nascimento, endereço e CEP: identificação, faixa etária e produção de indicadores territoriais.
- [x] Definir telefone e e-mail como opcionais.
- [x] Definir que não haverá comprovação adicional da titularidade do CPF nesta versão; os dados recuperados serão mascarados.
- [x] Definir que os dados mascarados serão editáveis e poderão ser atualizados pelo participante durante a inscrição, sem expor os valores completos já armazenados.
- [x] Definir que o participante não terá conta, login, senha ou verificação obrigatória por e-mail.
- [x] Definir que não haverá fluxo diferente para crianças nesta versão.
- [x] Definir que o administrador possui acesso total ao sistema, incluindo todas as funcionalidades do colaborador, além das funções administrativas e de relatórios.
- [x] Definir que o colaborador pode cadastrar e editar eventos e responsáveis, visualizar e editar o cadastro completo dos participantes, alterar dados pessoais e confirmar presenças, sem acesso aos relatórios.
- [x] Definir auditoria, retenção automatizada e exclusão/anonimização como fora do escopo da primeira versão.
- [x] Definir que a primeira versão não apresentará aviso de privacidade no formulário de cadastro ou inscrição.
- [x] Definir o tratamento de CPF inválido, CPF já cadastrado e formatos inválidos dos campos opcionais de e-mail e telefone.
- [x] Definir as validações de CEP, data de nascimento e preenchimento dos campos obrigatórios.

### Alterações previstas

- [x] Criar uma matriz única de campos com: nome, tipo, obrigatoriedade, finalidade, validação e visibilidade.
- [x] Remover a obrigatoriedade e o envio automático de código por e-mail; e-mail e telefone serão meios de contato opcionais.
- [x] Aplicar máscaras a todos os dados recuperados após a digitação de um CPF já cadastrado.
- [x] Permitir a edição dos campos mascarados, validando e salvando somente os campos substituídos por valores completos e preservando os demais dados armazenados.
- [x] Registrar como limitação conhecida que o fluxo não comprova a titularidade do CPF e não atende crianças sem CPF.
- [x] Aplicar autenticação e autorização por perfil: administrador com acesso total; colaborador com gestão de eventos, responsáveis, cadastros completos de participantes e presenças, mas sem relatórios ou exportações.
- [x] Não criar requisito funcional de aviso de privacidade nesta versão; registrar a limitação no escopo.
- [x] Não criar requisitos funcionais de auditoria, retenção automatizada ou exclusão/anonimização nesta versão; registrar a limitação no escopo.
- [x] Não implementar proteção adicional contra inscrições realizadas por terceiros nesta versão; registrar o risco como limitação conhecida.
- [x] Definir tratamento de CPF, e-mail ou telefone inválidos e registros duplicados.
- [x] Definir validações de CEP, data de nascimento e campos obrigatórios.

### Critério de conclusão

RN, RF, caso de uso, protótipo e futuro modelo de dados utilizam o mesmo conjunto de campos, aplicam o mascaramento definido, permitem a atualização cadastral durante a inscrição sem persistir máscaras e documentam que o CPF identifica o cadastro sem comprovar a identidade de quem realiza a inscrição.

## Etapa 3 — Normalizar requisitos de domínio `[x]`

### Objetivo

Transformar a lista de RN em regras únicas, numeradas e testáveis.

**Status:** concluída e ajustada. As 19 RN da V6 foram normalizadas em 39 regras únicas e testáveis na `docs/PI_NegraVisao_v8.docx`. Foram aplicadas as decisões sobre inscrição única por evento recorrente, encerramento da recorrência por quantidade de ocorrências ou data final, separação dos estados e tratamento de endereço por CEP. O CEP permanece obrigatório; CEP geral de município é aceito, campos não retornados pela API são preenchidos manualmente, indisponibilidade técnica permite preenchimento manual, CEP não encontrado exige correção e endereço sem número aceita `S/N`. O conflito histórico da RN10 já estava resolvido. Não restaram decisões pendentes nesta etapa, e as versões V6 e V7 foram preservadas.

### Alterações previstas

- [x] Confirmar a resolução do conflito histórico da RN10 e atribuir identificadores únicos a todas as regras.
- [x] Padronizar o formato `RN01`, sempre com dois dígitos.
- [x] Diferenciar recorrência, frequência, ocorrência, quantidade de ocorrências e duração.
- [x] Remover a redundância entre “único” e “1 vez”.
- [x] Substituir expressões vagas por critérios verificáveis; a recorrência termina por quantidade positiva de ocorrências ou por data final.
- [x] Separar estado do evento, disponibilidade das inscrições e estado da inscrição individual.
- [x] Definir os significados operacionais de `PROGRAMADO`, `REALIZADO`, `ABERTA`, `ENCERRADA`, `CONFIRMADA` e `INVALIDADA_POR_INATIVAÇÃO`.
- [x] Definir conflito de horários como sobreposição de intervalos, incluindo as ocorrências de eventos recorrentes.
- [x] Definir abertura e encerramento da janela de inscrição com data e horário locais do evento.
- [x] Definir a capacidade no evento e o cálculo das vagas em cada ocorrência da série recorrente.
- [x] Definir o efeito da inativação sobre inscrições existentes, vagas futuras e histórico.
- [x] Definir que nova inscrição pública não reativa automaticamente um participante inativo.
- [x] Criar regra de unicidade da inscrição por participante e evento, abrangendo todas as ocorrências da série.
- [x] Criar regra de concorrência para impedir que duas solicitações consumam a mesma última vaga.
- [x] Definir os campos estruturados do endereço e o tratamento de CEP geral, consulta à API, indisponibilidade do serviço, CEP não encontrado e endereço sem número.

### Critério de conclusão

Cada RN possui um único identificador, uma única interpretação e condições que podem ser transformadas em testes.

## Etapa 4 — Reconstruir requisitos funcionais e rastreabilidade `[x]`

### Objetivo

Garantir que todas as funcionalidades do escopo tenham RF e que cada RF aponte para as regras corretas.

**Status:** concluída e validada. A `docs/PI_NegraVisao_v9.docx` contém 15 RF sequenciais e testáveis, derivados das 51 RN vigentes, e uma matriz de rastreabilidade RN–RF–UC–tela–entidade. Foram incorporados dados pessoais compartilhados entre papéis, inscrição de usuários internos, publicação e ocultação, cancelamentos de eventos e inscrições, recorrência sem previsão de fim, presença e faltas por ocorrência, liberação de vagas, reativação interna e relatórios em tela, PDF e CSV. As contradições objetivas de RN04 e RN16 foram resolvidas por alterações controladas nas RN. Não restaram decisões pendentes. A V8 foi preservada.

### Alterações previstas

- [x] Manter fora da lista o antigo RF03 “Gerar notificação em tela”; a confirmação visual e o código são tratados por RF11.
- [x] Renumerar os requisitos como RF01 a RF15 e atualizar as referências diretamente afetadas.
- [x] Padronizar cada RF com identificador, nome, ator, entradas, validações, resultado e regras relacionadas.
- [x] Usar `Evento` como conceito principal e curso, oficina e palestra somente como tipos de evento.
- [x] Relacionar a exibição do código à RN22, que exige código único para a inscrição confirmada.
- [x] Aplicar a validade do e-mail opcional, definida na RN33, aos RF que cadastram ou alteram a pessoa.
- [x] Não criar fluxo de falha de envio: não existe envio automático de e-mail nesta versão, e RF11 exibe a confirmação em tela.
- [x] Relacionar os relatórios às regras de autenticação, autorização, agregação territorial e conteúdo dos relatórios.
- [x] Especificar tela, PDF e CSV, filtros, dados apresentados e permissão exclusiva do administrador em RF15.
- [x] Não criar proteção contra inscrição por terceiros; a primeira versão não comprova a titularidade do CPF e mantém esse risco como limitação conhecida.
- [x] Definir cancelamento e reativação interna da inscrição, cancelamento do evento, preservação do histórico e liberação de vagas futuras.
- [x] Manter cadastro e edição de eventos e acrescentar cancelamento quando existirem ocorrências futuras.
- [x] Definir evento novo como `OCULTO` e permitir publicação ou ocultação por colaborador ou administrador.

### Matriz de rastreabilidade produzida

| RN | RF relacionado | UC relacionado | Tela/Protótipo | Entidade futura |
|---|---|---|---|---|
| RN01–RN05 | RF01, RF02 | UC03 e UC09 | Página pública; painel de eventos | Evento, Ocorrência |
| RN06–RN09 | RF02, RF05 | UC02, UC03 | Painel de responsáveis e eventos | Pessoa, Responsável, Evento, Ocorrência |
| RN10–RN12 | RF10, RF12 | UC05 e UC14 | Formulário de inscrição; lista de inscritos | Pessoa, Evento, Inscrição |
| RN13–RN15 | RF01, RF02, RF10, RF12 | UC03, UC05, UC09 e UC14 | Detalhes do evento; inscrição; lista de inscritos | Evento, Ocorrência, Inscrição |
| RN16–RN18 | RF01–RF04, RF10 | UC03, UC05, UC09, UC12 e UC13 | Página pública; painel de eventos | Evento, Ocorrência |
| RN19–RN22 | RF10–RF14 | UC01, UC05, UC07 e UC14 | Inscrição; confirmação; presença; participante | Pessoa, Inscrição, Presença |
| RN23–RN34 | RF05–RF07, RF09, RF10 | UC02, UC04 a UC06 e UC11 | Cadastros; formulário de inscrição | Pessoa, Endereço, Papel |
| RN35–RN38 | RF02–RF05, RF07–RF09, RF12–RF15 | UC01 a UC03, UC06 a UC08 e UC10 a UC14 | Login e painel interno | Pessoa, Usuário, Perfil, Papel |
| RN39 | RF15 | UC08 | Relatórios | Pessoa, Endereço, Relatório |
| RN40 | RF05, RF06, RF09, RF10 | UC02, UC04, UC05 e UC11 | Cadastros e inscrição | Pessoa, Papel, Usuário |
| RN41–RN42 | RF01–RF03, RF10 | UC03, UC05, UC09 e UC12 | Página pública; painel de eventos | Evento |
| RN43–RN44 | RF04, RF12 | UC13 e UC14 | Painel de evento e inscritos | Evento, Ocorrência, Inscrição |
| RN45 | RF10, RF12 | UC05 e UC14 | Lista de inscritos | Inscrição |
| RN46–RN49 | RF12, RF13 | UC01 e UC14 | Lista de presença e inscritos | Ocorrência, Inscrição, Presença |
| RN50 | RF10, RF12 | UC05 e UC14 | Lista de inscritos | Inscrição |
| RN51 | RF15 | UC08 | Relatórios | Relatório, Evento, Inscrição, Presença, Pessoa |

### Critério de conclusão

Não existem saltos de numeração, referências inexistentes ou funcionalidades do escopo sem RF. Todos os relacionamentos RN–RF são justificáveis.

## Etapa 5 — Revisar atores e casos de uso `[x]`

### Objetivo

Alinhar atores, permissões e casos de uso com os RFs aprovados.

**Status:** concluída e validada. A `docs/PI_NegraVisao_v10.docx` define cinco atores — Público, Participante, Colaborador, Administrador e Serviço de endereços — e mantém Responsável como papel de domínio sem acesso ao sistema. O catálogo foi normalizado em 14 casos de uso sequenciais, com cobertura integral de RF01 a RF15. Administrador foi definido como especialização funcional de Colaborador; autenticação tornou-se UC próprio e pré-condição das operações internas; confirmação em tela e geração do código foram consolidadas em UC05; o termo oficial passou a ser `código da inscrição`. Não existe serviço de e-mail nesta versão e não restaram decisões pendentes. A V9 foi preservada.

### Alterações previstas

- [x] Retirar “Sistema” como ator secundário do próprio sistema; RF11 e UC05 tratam confirmação e código como resultados da interação do participante.
- [x] Não incluir serviço de e-mail: o escopo aprovado não prevê envio automático; Serviço de endereços é o único ator externo necessário nesta versão.
- [x] Atualizar o catálogo textual para `Participante`; referências históricas no diagrama ficam registradas para a Etapa 7.
- [x] Usar `Administrador` por extenso no catálogo; referências históricas no diagrama ficam registradas para a Etapa 7.
- [x] Definir Administrador como especialização funcional de Colaborador, herdando suas permissões.
- [x] Criar UC10 — Autenticar usuário interno para colaborador e administrador.
- [x] Consolidar confirmação em tela e geração de código em UC05 — Realizar inscrição.
- [x] Padronizar o termo `código da inscrição`.
- [x] Consolidar visualização e exportação dos relatórios em UC08, conforme RF15.
- [x] Incluir UC01 a UC14 para cobrir integralmente RF01 a RF15.
- [x] Registrar as relações entre UC sem criar `include` ou `extend` artificiais: UC05 utiliza UC04; autenticação é pré-condição das operações internas; as demais relações serão representadas no diagrama da Etapa 7.

### Critério de conclusão

A lista de atores e as descrições de UC usam os mesmos nomes, permissões e funcionalidades dos RFs; a especificação aprovada fornece a referência obrigatória para atualizar o diagrama na Etapa 7.

## Etapa 6 — Reescrever o caso de uso Realizar inscrição `[x]`

### Objetivo

Produzir um fluxo principal e fluxos alternativos completos, numerados e verificáveis.

**Status:** concluída e validada. A `docs/PI_NegraVisao_v11.docx` identifica o caso detalhado como UC05 — Realizar inscrição e apresenta FP01–FP11, A01–A06, E01–E09, pré-condições P01–P03 e pós-condições de sucesso S01–S03 e de insucesso F01–F03. O fluxo cobre CPF novo ou existente, dados mascarados, endereço e Serviço de endereços, participante inativo, inscrição duplicada, estados sem reativação pública, prazo, lotação, concorrência, conflito e falha técnica. E-mail permanece opcional e não existe envio automático. Todas as referências apontam para etapas válidas, não restaram decisões pendentes e a V10 foi preservada.

### Alterações previstas

- [x] Identificar explicitamente o caso detalhado como UC05 — Realizar inscrição.
- [x] Usar nomenclatura uniforme em português, sem `happy path` ou mistura desnecessária de idiomas.
- [x] Numerar o fluxo principal como FP01 a FP11.
- [x] Substituir a formulação histórica por “seleciona um evento publicado e aciona a opção Inscrever-se”.
- [x] Usar `participante` em vez de `cliente`.
- [x] Manter e-mail opcional; quando informado, apenas validá-lo, sem envio automático.
- [x] Limitar os dados aos campos aprovados na Etapa 2.
- [x] Substituir referências vagas por referências válidas a FP, A e E.
- [x] Numerar alternativas como A01–A06 e exceções como E01–E09.
- [x] Definir comportamentos para CPF inválido, dados inválidos, cadastro existente e inscrição duplicada.
- [x] Definir comportamentos para evento lotado, concorrência pela última vaga, prazo encerrado e conflito de horário.
- [x] Não criar fluxo de falha de envio de e-mail, pois não existe envio automático nesta versão.
- [x] Definir E09 para falha técnica, sem inscrição, vaga ou código em estado parcial e sem duplicidade na repetição.
- [x] Definir pré-condições objetivas para evento programado e publicado, inscrições abertas, CPF disponível e revalidação da aptidão.
- [x] Definir pós-condições observáveis de sucesso, insucesso, abandono e repetição após falha.
- [x] Impedir reativação pública de participante inativo e permitir reutilização de inscrição invalidada somente após reativação administrativa da pessoa e novas validações.

### Critério de conclusão

Cada caminho termina em um estado conhecido, todas as referências apontam para etapas existentes e o fluxo cobre as regras relacionadas.

## Etapa 7 — Atualizar os diagramas `[x]`

**Status:** concluída e validada. A `docs/PI_NegraVisao_v12.docx` substituiu os dois diagramas históricos por quatro diagramas de atividades — consulta e inscrição pública, gestão de eventos, inscrições e presença, administração e relatórios — e duas visões do diagrama de casos de uso — área pública e painel interno. Os diagramas utilizam o vocabulário aprovado, distribuem as ações por responsável, apresentam a fronteira Sistema Negra Visão, cobrem UC01 a UC14 e representam Administrador como especialização funcional de Colaborador. UC05 utiliza UC04; autenticação é indicada como pré-condição das operações internas; não há relações `extend` artificiais, serviço de e-mail ou notificações automáticas. A correção visual posterior foi consolidada em `docs/PI_NegraVisao_v13.docx`: somente formas, conectores e posições dos diagramas de atividades foram ajustados para impedir textos fora das formas, sem alteração de conteúdo. As versões V11 e V12 foram preservadas.

### Diagrama de atividades

- [x] Corrigir “Sitema/Site”, utilizando a raia `Sistema`.
- [x] Atualizar “Aluno” para `Participante`.
- [x] Atualizar “Cadastra palestrante” para `Cadastrar ou editar evento e responsável`.
- [x] Colocar cada atividade na raia de quem efetivamente a executa.
- [x] Separar o fluxo abrangente em quatro diagramas: consulta e inscrição, gestão de eventos, inscrições e presença, administração e relatórios.
- [x] Reduzir cruzamentos de linhas e aumentar a legibilidade.
- [x] Exportar os quatro diagramas em 1800 × 920 pixels, sem marca de ferramenta.

### Diagrama de casos de uso

- [x] Identificar as duas fronteiras como `Sistema Negra Visão - Área pública` e `Sistema Negra Visão - Painel interno`.
- [x] Atualizar atores e casos conforme a Etapa 5, cobrindo UC01 a UC14.
- [x] Remover casos e atores ainda nomeados com “aluno”.
- [x] Representar Administrador como especialização de Colaborador, UC05 incluindo UC04 e autenticação como pré-condição; não criar relações `extend` artificiais.
- [x] Alinhar relatórios aos RFs aprovados e remover notificações ou envio por e-mail não previstos.
- [x] Exportar a visão pública em 1800 × 800 pixels e a visão interna em 1800 × 1200 pixels, sem marca de ferramenta.

### Critério de conclusão

Os diagramas podem ser compreendidos sem consultar uma versão anterior e não contradizem requisitos ou casos de uso.

## Etapa 8 — Revisar os protótipos `[x]`

**Status:** concluída e validada. A `docs/PI_NegraVisao_v14.docx` substitui o protótipo provisório por dez pranchas de média fidelidade, quatro da área pública e seis do painel interno. As pranchas cobrem RF01 a RF15 e UC01 a UC14 e representam campos aprovados, obrigatoriedade, máscaras editáveis, consulta assistida de CEP, estados de sucesso e impedimento, permissões, cancelamentos, presença e relatórios. Não foram introduzidos login público, cancelamento público, envio automático por e-mail ou outras funcionalidades fora do escopo. Não restaram decisões pendentes, e a V13 foi preservada. Após a validação, as imagens dos diagramas na V14 foram ajustadas manualmente; a V14 atualizada é a fonte de verdade para as próximas etapas e essas imagens não devem ser regeneradas automaticamente.

### Objetivo

Fazer com que as telas representem os requisitos aprovados, e não campos ou textos provisórios.

### Alterações previstas

- [x] Substituir “imagem do curso”, “nome do curso”, “New Services” e “Project Reports” por conteúdo coerente.
- [x] Padronizar caixa e grafia de CPF, nome, e-mail, telefone, CEP e endereço.
- [x] Exibir somente os campos aprovados na matriz de dados.
- [x] Incluir legenda para campos obrigatórios.
- [x] Melhorar contraste de campos, textos e botões.
- [x] Não depender apenas das cores verde/vermelha para comunicar ações.
- [x] Mostrar validações de erro e mensagens orientativas.
- [x] Na tela de sucesso, exibir evento, data e código da inscrição, sem indicar envio automático por e-mail.
- [x] Incluir próximo passo, como voltar aos eventos ou consultar inscrição.
- [x] Criar telas administrativas mínimas para gestão, relatórios e presença.

### Critério de conclusão

Cada campo e ação exibidos possui RF correspondente, e cada RF com interface possui uma tela ou estado representado.

## Etapa 9 — Criar os modelos técnicos ausentes

**Dependência registrada:** o modelo conceitual e o modelo lógico do banco de dados serão obtidos do memorial `banco de dados/Implementação do projeto de banco de dados Negra visao.docx`, que ainda está em elaboração. A Etapa 9 não deve recriar esses modelos em paralelo nem consolidá-los antes que o memorial seja concluído e validado. Quando autorizado, a etapa deverá integrar e conferir o memorial contra os requisitos, protótipos e diagramas aprovados.

### Diagrama de classes

- [ ] Integrar e validar o modelo conceitual proveniente do memorial `banco de dados/Implementação do projeto de banco de dados Negra visao.docx`.
- [ ] Avaliar entidades como Participante, Usuário, Perfil, Evento, Ocorrência, Responsável, Inscrição e Presença.
- [ ] Definir atributos, identificadores, relacionamentos e multiplicidades.
- [ ] Representar recorrência e ocorrências sem duplicar conceitos.
- [ ] Representar status como conceitos controlados.

### Modelo lógico do banco

- [ ] Incorporar e conferir o modelo lógico proveniente do mesmo memorial, após sua conclusão e validação.
- [ ] Verificar tabelas, chaves, unicidades e nulabilidades somente com base no memorial aprovado.
- [ ] Definir PKs, FKs, unicidade e nulabilidade.
- [ ] Definir unicidade de CPF e do número/código de inscrição.
- [ ] Criar restrição para impedir inscrição duplicada na mesma ocorrência.
- [ ] Definir como capacidade e presença serão registradas.
- [ ] Definir campos de auditoria e inativação sem apagar histórico.
- [ ] Revisar quais dados pessoais são realmente armazenados.

### Critério de conclusão

Todo dado citado nos RN/RF possui representação no modelo, e toda entidade/coluna do modelo tem justificativa no escopo ou nos requisitos.

## Etapa 10 — Revisar redação e organização acadêmica

### Alterações previstas

- [ ] Atualizar o título de V2 para V3 ou remover a versão do título e mantê-la apenas no histórico.
- [ ] Substituir “Histórico de Revisão” por uma tabela real de versão, data, alteração e responsável.
- [ ] Mover a distribuição de tarefas para uma seção de planejamento/contribuições, caso seja exigida.
- [ ] Corrigir “Fluxograma use em case”.
- [ ] Corrigir “Protótipo média fidelidade” para “Protótipo de média fidelidade”.
- [ ] Padronizar “Caso de Uso”, evitando “Use Case” e “Use case”.
- [ ] Corrigir “Requisitos De Domínio” para “Requisitos de domínio”.
- [ ] Corrigir frases como “Ao se interessar, acessa o evento e para realizar inscrição”.
- [ ] Corrigir “mudara” para “mudará”.
- [ ] Corrigir “Caso o participante já possuir” para “Caso o participante já possua”.
- [ ] Corrigir “Caso não houver” para “Caso não haja”.
- [ ] Corrigir “exibi” para “exibe”.
- [ ] Corrigir “o sistema verificar” para “o sistema verifica”.
- [ ] Corrigir “validação e tela” para “validação em tela”.
- [ ] Eliminar o fragmento “Assim como inativar participantes”.
- [ ] Acrescentar pontuação final consistente às RN e descrições.
- [ ] Avaliar a retirada dos e-mails pessoais caso o documento seja publicado.

### Critério de conclusão

O documento usa português consistente, não mistura idiomas desnecessariamente e não contém frases incompletas ou referências ambíguas.

## Etapa 11 — Corrigir estrutura, paginação e acessibilidade

### Alterações previstas

- [ ] Aplicar estilo de título ao título principal.
- [ ] Criar hierarquia real com Título, Heading 1, Heading 2 e Heading 3.
- [ ] Manter títulos junto ao primeiro parágrafo, tabela ou imagem da seção.
- [ ] Evitar divisões de RF e UC no meio de frases.
- [ ] Impedir que “Protótipo” fique sozinho no fim da página.
- [ ] Repetir cabeçalhos em tabelas que atravessam páginas.
- [ ] Redimensionar tabelas para a largura útil de 5,91 polegadas.
- [ ] Remover alturas fixas ou espaços vazios excessivos nas tabelas.
- [ ] Adicionar legendas aos diagramas e protótipos.
- [ ] Adicionar texto alternativo às seis imagens.
- [ ] Marcar semanticamente as linhas de cabeçalho das tabelas aplicáveis.
- [ ] Avaliar sumário automático e numeração de páginas.
- [ ] Substituir formatação direta por estilos consistentes.
- [ ] Revisar a posição do logotipo flutuante em Word e LibreOffice.

### Critério de conclusão

Todas as páginas renderizam sem corte, sobreposição, títulos órfãos, grandes vazios injustificados ou tabelas fora das margens. A navegação por títulos e leitores de tela é utilizável.

## Etapa 12 — Validação final cruzada

### Checklist de conteúdo

- [ ] Escopo e objetivo correspondem aos RFs.
- [ ] Todas as RN têm identificador único.
- [ ] Todos os RFs têm identificador único.
- [ ] Todos os UCs correspondem a RFs existentes.
- [ ] Diagramas usam os mesmos atores e nomes dos textos.
- [ ] Protótipos usam os mesmos campos dos requisitos.
- [ ] Modelo de classes e modelo lógico representam as mesmas entidades.
- [ ] Não existem referências a “Aluno” se o termo oficial for “Participante”.
- [ ] Não existem referências antigas a RN/RF renumerados.
- [ ] Não existem funcionalidades prometidas sem especificação.

### Checklist de qualidade do arquivo

- [ ] Aceitar ou remover revisões pendentes, se surgirem durante a edição.
- [ ] Resolver comentários de revisão.
- [ ] Atualizar sumário e campos automáticos.
- [ ] Renderizar todas as páginas em PNG/PDF.
- [ ] Inspecionar todas as páginas em escala de 100%.
- [ ] Comparar paginação no Word e no renderizador final.
- [ ] Verificar margens, tabelas, diagramas e quebras de página.
- [ ] Executar auditoria de acessibilidade.
- [ ] Conferir metadados, autor, versão e nome do arquivo.
- [ ] Produzir uma versão final limpa e uma cópia de revisão, se necessário.

### Critério de conclusão

A matriz RN–RF–UC–tela–entidade não possui lacunas, e a versão renderizada não apresenta defeitos visuais ou de acessibilidade conhecidos.

---

# Ordem recomendada de execução

1. Etapa 1 — Escopo e vocabulário
2. Etapa 2 — Dados, segurança e LGPD
3. Etapa 3 — Requisitos de domínio
4. Etapa 4 — Requisitos funcionais e rastreabilidade
5. Etapas 5 e 6 — Casos de uso
6. Etapa 7 — Diagramas
7. Etapa 8 — Protótipos
8. Etapa 9 — Modelos técnicos
9. Etapa 10 — Redação
10. Etapa 11 — Layout e acessibilidade
11. Etapa 12 — Validação final

## Próxima ação recomendada

Prosseguir para a **Etapa 9 — Criar os modelos técnicos ausentes**, usando `docs/PI_NegraVisao_v14.docx`, as decisões consolidadas nas Etapas 1 a 8 e os impactos registrados para modelagem como referências.
