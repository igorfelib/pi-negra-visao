<p align="center">
  <img src="frontend/img/logo_completo.svg" alt="Logotipo da Associação Negra Visão" width="220">
</p>

# Negra Visão

Projeto Integrador da FATEC Atibaia para apoiar a divulgação de eventos da Associação Negra Visão e documentar o fluxo de inscrição, a gestão interna e a modelagem de dados.

> Projeto acadêmico em desenvolvimento. A versão atual é um protótipo front-end e não deve ser usada para armazenar dados reais ou sensíveis.

## Sobre o projeto

A Associação Negra Visão atua em Atibaia, São Paulo, na difusão da cultura preta, no fortalecimento da identidade negra e no combate ao racismo. O projeto especifica uma página pública para consulta e inscrição em eventos e um painel interno para a equipe da associação.

O repositório contém um protótipo front-end demonstrativo e a documentação do sistema. O memorial de banco de dados está finalizado; a aplicação ainda não possui servidor nem integração com esse banco.

## Estado atual

| Área | Situação | O que está disponível |
| --- | --- | --- |
| Documentação do sistema | Atualizada | Requisitos, casos de uso, diagramas, protótipos e modelo de classes em PI_NegraVisao_v16.docx |
| Protótipo front-end | Demonstrativo | Páginas públicas e painel interno; não implementa todas as regras aprovadas na documentação |
| Persistência do protótipo | Local | Dados armazenados somente no `localStorage` do navegador |
| Memorial e modelo de dados | Finalizados | Memorial, MER, script SQL e dicionários de dados disponíveis em `banco de dados/` |
| Backend e integração com banco | Não implementados | O protótipo não deve ser usado para cadastrar dados reais ou sensíveis |

## Funcionalidades

- Página inicial responsiva com identidade visual da Negra Visão.
- Cards de eventos com modal de detalhes e acesso ao formulário de inscrição.
- Formulário com validação de CPF e campos obrigatórios.
- Demonstração do reaproveitamento de dados de uma pessoa já cadastrada no navegador.
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
├── docs/                  # Documentação do Projeto Integrador e diagramas
├── banco de dados/        # Memorial final, MER, SQL e dicionários de dados
├── frontend/
│   ├── css/               # Estilos das páginas
│   ├── img/               # Logotipos, padrões e imagens de eventos
│   ├── js/                # Interações, validações e persistência local
│   ├── index.html         # Portal institucional
│   ├── inscricao.html     # Formulário de inscrição
│   ├── login.html         # Login demonstrativo
│   └── Admin.html         # Painel administrativo
└── README.md
```

## Armazenamento de dados

O protótipo mantém os dados somente no navegador:

- `alunos`: chave legada do protótipo para dados de pessoas e inscrições; na documentação atual, o termo de domínio é participante;
- `nv_responsaveis`: responsáveis cadastrados;
- `nv_eventos`: eventos cadastrados;
- `eventoSelecionadoInscricao`: evento escolhido antes de abrir o formulário.

Limpar os dados do site no navegador remove esses registros. Como ainda não há servidor ou banco de dados, informações salvas em um dispositivo não aparecem em outro.

## Documentação

- [Projeto Integrador — versão 16](docs/PI_NegraVisao_v16.docx)
- [Memorial final de implementação do banco de dados](<banco de dados/banco de dados Negra visao v2.docx>)
- [Modelo entidade-relacionamento em PNG](<banco de dados/MER.png>) · [em SVG](<banco de dados/MER.svg>)
- [Script SQL](<banco de dados/sql_V1.sql>)
- [Dicionário semântico de dados](<banco de dados/Dicionario_Dados_Semantico_Negra_Visao_Compatível_SQL.xlsx>)
- [Dicionário técnico de dados](<banco de dados/Dicionario_Dados_Tecnico_Negra_Visao_Compatível_SQL.xlsx>)
- [Diagrama de classes em SVG](docs/diagrama_classes_negra_visao.svg)
- [Relatório histórico da revisão da V2](docs/Relatorio_Revisao_PI_NegraVisao_v2.md)

## Próximas etapas

- Implementar backend, autenticação interna e integração com o modelo de dados final.
- Alinhar o protótipo às regras de inscrição, capacidade, recorrência, cancelamento e presença.
- Implementar relatórios administrativos com acesso restrito ao perfil administrador.
- Aplicar controles de segurança e privacidade antes de qualquer uso com dados reais.

## Equipe

- Bruno de Paula Almeida
- Igor Felipe Barbosa
- Luiza Kovacs Viana
- Marisol E. F. Sawaya
- Victor Lyan Fronio Oliveira

