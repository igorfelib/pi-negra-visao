<p align="center">
  <img src="frontend/img/logo_completo.svg" alt="Logotipo da Associação Negra Visão" width="220">
</p>

# Negra Visão

Plataforma web desenvolvida como Projeto Integrador da FATEC Atibaia para apoiar a divulgação das ações da Associação Negra Visão e demonstrar o fluxo de inscrição em eventos, o cadastro de responsáveis e a gestão básica de atividades.

> Projeto acadêmico em desenvolvimento. A versão atual é um protótipo front-end e não deve ser usada para armazenar dados reais ou sensíveis.

## Sobre o projeto

A Associação Negra Visão atua em Atibaia, São Paulo, na difusão da cultura preta, no fortalecimento da identidade negra e no combate ao racismo. O projeto propõe um canal digital centralizado para apresentar a instituição, divulgar eventos e simplificar a inscrição da comunidade.

O protótipo também inclui uma área administrativa demonstrativa para cadastrar responsáveis e eventos e consultar as inscrições salvas no navegador.

## Estado atual

| Área | Situação | O que está disponível |
| --- | --- | --- |
| Portal institucional | Implementada | Apresentação da associação, agenda de eventos, memorial, biblioteca e conteúdo cultural |
| Inscrição em eventos | Implementada no protótipo | Seleção do evento, validação de campos e CPF, preenchimento automático de cadastro existente e bloqueio de inscrição duplicada no mesmo evento |
| Painel administrativo | Implementado no protótipo | Cadastro, edição e exclusão de responsáveis e eventos; consulta e exclusão de inscrições |
| Persistência | Local | Dados armazenados no `localStorage` do navegador |
| Integrações e serviços | Planejados | Banco de dados, autenticação segura, envio de e-mail, validação de presença e exportação de relatórios |

## Funcionalidades

- Página inicial responsiva com identidade visual da Negra Visão.
- Cards de eventos com modal de detalhes e acesso ao formulário de inscrição.
- Formulário com validação de CPF e campos obrigatórios.
- Reaproveitamento dos dados de um participante já cadastrado no navegador.
- Bloqueio de inscrições repetidas para o mesmo CPF e evento.
- Login demonstrativo para acesso ao painel administrativo.
- Cadastro e manutenção de responsáveis e eventos.
- Consulta das inscrições registradas localmente.
- Menu adaptado para dispositivos móveis.

## Tecnologias

- HTML5
- CSS3
- JavaScript
- Web Storage API (`localStorage`)

O projeto não utiliza framework, processo de build ou dependências externas de execução.

## Como executar

Clone o repositório e inicie um servidor HTTP na pasta `frontend`:

```bash
git clone https://github.com/igorfelib/pi-negra-visao.git
cd pi-negra-visao/frontend
python -m http.server 8000
```

Depois, acesse [http://localhost:8000](http://localhost:8000) no navegador.

Também é possível abrir `frontend/index.html` diretamente, mas um servidor local reproduz melhor a navegação entre as páginas.

### Acesso administrativo demonstrativo

- Usuário: `admin`
- Senha: `admin`

As credenciais estão no código-fonte e servem apenas para demonstração acadêmica. Elas não representam um mecanismo de autenticação seguro.

## Estrutura do repositório

```text
pi-negra-visao/
├── docs/                  # Documentação do Projeto Integrador
├── frontend/
│   ├── css/               # Estilos das páginas
│   ├── img/               # Logotipos, padrões e imagens de eventos
│   ├── js/                # Interações, validações e persistência local
│   ├── index.html         # Portal institucional
│   ├── inscricao.html     # Formulário de inscrição
│   ├── login.html         # Login demonstrativo
│   └── Admin.html         # Painel administrativo
├── LICENSE
└── README.md
```

## Armazenamento de dados

O protótipo mantém os dados somente no navegador:

- `alunos`: inscrições e dados dos participantes;
- `nv_responsaveis`: responsáveis cadastrados;
- `nv_eventos`: eventos cadastrados;
- `eventoSelecionadoInscricao`: evento escolhido antes de abrir o formulário.

Limpar os dados do site no navegador remove esses registros. Como ainda não há servidor ou banco de dados, informações salvas em um dispositivo não aparecem em outro.

## Documentação

- [Documentação revisada do Projeto Integrador](docs/PI_NegraVisao_v3.docx)
- [Versão anterior](docs/PI_NegraVisao_v2.docx)

## Próximas etapas

- Implementar API e banco de dados.
- Substituir o login demonstrativo por autenticação e autorização por perfil.
- Aplicar regras de capacidade, período de inscrição e conflito de horários.
- Enviar confirmação e código de inscrição por e-mail.
- Registrar presença sem excluir o histórico do participante.
- Exportar relatórios de inscritos e presença.
- Adicionar testes automatizados, validações de segurança e adequação à LGPD.

## Equipe

- Bruno de Paula Almeida
- Igor Felipe Barbosa
- Luiza Kovacs Viana
- Marisol E. F. Sawaya
- Victor Lyan Fronio Oliveira

## Licença

Este projeto está disponível sob a [Licença MIT](LICENSE).
