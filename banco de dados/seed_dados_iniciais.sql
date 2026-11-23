-- Script de Carga Inicial (Seed) — Negra Visão
-- Compatível com sql_V1.sql (Schema: negra_visao)

USE `negra_visao`;

-- 1. Papéis de domínio (RN40)
INSERT IGNORE INTO `papel` (`id_papel`, `nm_papel`) VALUES
  (1, 'PARTICIPANTE'),
  (2, 'RESPONSAVEL'),
  (3, 'COLABORADOR'),
  (4, 'ADMINISTRADOR');

-- 2. Pessoa e Endereço da Coordenação (Administrador e Responsável padrão)
INSERT IGNORE INTO `pessoa` (`id_pessoa`, `nm_pessoa`, `nr_cpf`, `dt_nascimento`, `ds_email`, `nr_telefone`, `st_pessoa`) VALUES
  (1, 'Coordenação Negra Visão', '11144477735', '1988-11-20', 'contato@negravisao.org.br', '11999990000', 'ATIVA');

INSERT IGNORE INTO `endereco` (`id_endereco`, `id_pessoa`, `nr_cep`, `ds_logradouro`, `nr_imovel`, `ds_complemento`, `ds_bairro`, `ds_municipio`, `sg_uf`) VALUES
  (1, 1, '12940000', 'Praça da Matriz', '100', 'Sede Cultural', 'Centro', 'Atibaia', 'SP');

INSERT IGNORE INTO `pessoa_papel` (`id_pessoa`, `id_papel`) VALUES
  (1, 2),
  (1, 4);

-- 3. Usuário Interno padrão (Login: admin / Senha: admin)
INSERT IGNORE INTO `usuarioInterno` (`id_usuarioInterno`, `id_pessoa`, `nm_login`, `nm_senha`, `st_situacao_acesso`) VALUES
  (1, 1, 'admin', 'admin', 'ATIVO');

-- 4. Responsável padrão
INSERT IGNORE INTO `responsavel` (`id_responsavel`, `id_pessoa`, `ds_responsavel`) VALUES
  (1, 1, 'Coordenação Cultural e Pedagógica da Associação Negra Visão');

-- 5. Eventos Culturais Iniciais (PUBLICADOS e PROGRAMADOS)
INSERT IGNORE INTO `evento` (`id_evento`, `ds_titulo`, `ds_evento`, `tp_evento`, `ds_local`, `st_classificacao`, `st_frequencia`, `ds_modalidade_duracao`, `nr_quantidade_total_ocorrencias`, `dt_final`, `nr_capacidade`, `dt_hora_abertura_inscricao`, `dt_hora_encerramento_inscricao`, `nr_limite_faltas_consecutivas`, `st_estado_evento`, `st_visibilidade`) VALUES
  (1, 'CAPOEIRA', 'Vivência aberta com roda, música e movimento para celebrar a capoeira como prática de resistência, cultura e formação coletiva.', 'CURSO', 'Sede Negra Visão - Atibaia/SP', 'RECORRENTE', 'SEMANAL', 'SEM_PREVISAO_DE_FIM', NULL, NULL, 25, '2026-01-01 08:00:00', NULL, 3, 'PROGRAMADO', 'PUBLICADO'),
  (2, 'Colo de mãe', 'Encontro dedicado ao acolhimento, à escuta e às vivências afetivas que fortalecem a comunidade a partir do cuidado.', 'OFICINA', 'Sede Negra Visão - Atibaia/SP', 'UNICO', NULL, NULL, NULL, NULL, 25, '2026-01-01 08:00:00', '2026-11-29 16:00:00', NULL, 'PROGRAMADO', 'PUBLICADO'),
  (3, 'CINE VISÃO', 'Sessão de cinema com conversa após a exibição, destacando narrativas negras e o poder do audiovisual como memória.', 'PALESTRA', 'Sede Negra Visão - Atibaia/SP', 'UNICO', NULL, NULL, NULL, NULL, 25, '2026-01-01 08:00:00', '2026-11-06 16:00:00', NULL, 'PROGRAMADO', 'PUBLICADO'),
  (4, 'Dança: samba rock', 'Oficina e baile para aprender passos, experimentar ritmos e celebrar o samba rock como expressão de identidade.', 'OFICINA', 'Sede Negra Visão - Atibaia/SP', 'UNICO', NULL, NULL, NULL, NULL, 25, '2026-01-01 08:00:00', '2026-11-30 12:00:00', NULL, 'PROGRAMADO', 'PUBLICADO'),
  (5, 'Teatro: Ana Julia', 'Apresentação teatral com foco em afeto, denúncia e reflexão sobre experiências do cotidiano e da vivência negra.', 'OFICINA', 'Sede Negra Visão - Atibaia/SP', 'UNICO', NULL, NULL, NULL, NULL, 25, '2026-01-01 08:00:00', '2027-07-12 12:00:00', NULL, 'PROGRAMADO', 'PUBLICADO'),
  (6, 'QUINTA COLETIVA', 'Roda de encontro para diálogo, criação conjunta e fortalecimento da participação comunitária por meio da arte.', 'PALESTRA', 'Sede Negra Visão - Atibaia/SP', 'UNICO', NULL, NULL, NULL, NULL, 25, '2026-01-01 08:00:00', '2026-11-13 17:00:00', NULL, 'PROGRAMADO', 'PUBLICADO'),
  (7, 'Aya yoga', 'Prática guiada de yoga com foco em respiração, presença e bem-estar para corpo e mente em um ambiente acolhedor.', 'CURSO', 'Sede Negra Visão - Atibaia/SP', 'RECORRENTE', 'SEMANAL', 'SEM_PREVISAO_DE_FIM', NULL, NULL, 25, '2026-01-01 08:00:00', NULL, 3, 'PROGRAMADO', 'PUBLICADO');

-- 6. Vínculo Evento x Responsável
INSERT IGNORE INTO `evento_responsavel` (`id_evento`, `id_responsavel`) VALUES
  (1, 1), (2, 1), (3, 1), (4, 1), (5, 1), (6, 1), (7, 1);

-- 7. Ocorrências Iniciais
INSERT IGNORE INTO `ocorrencia` (`id_ocorrencia`, `id_evento`, `dt_hora_inicio`, `dt_hora_fim`, `st_cancelada`, `st_lista_presenca_finalizada`) VALUES
  (1, 1, '2027-06-14 19:30:00', '2027-06-14 21:00:00', 0, 0),
  (2, 1, '2027-06-21 19:30:00', '2027-06-21 21:00:00', 0, 0),
  (3, 2, '2026-11-29 18:30:00', '2026-11-29 20:30:00', 0, 0),
  (4, 3, '2026-11-06 18:30:00', '2026-11-06 21:00:00', 0, 0),
  (5, 4, '2026-11-30 14:00:00', '2026-11-30 16:30:00', 0, 0),
  (6, 5, '2027-07-12 15:00:00', '2027-07-12 17:00:00', 0, 0),
  (7, 6, '2026-11-13 19:00:00', '2026-11-13 21:00:00', 0, 0),
  (8, 7, '2027-10-16 19:00:00', '2027-10-16 20:30:00', 0, 0),
  (9, 7, '2027-10-23 19:00:00', '2027-10-23 20:30:00', 0, 0);
