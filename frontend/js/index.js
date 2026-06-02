const eventos = [
  {
    titulo: "CAPOEIRA",
    meta: "14 Junho · 19:30",
    texto:
      "Vivência aberta com roda, música e movimento para celebrar a capoeira como prática de resistência, cultura e formação coletiva.",
  },
  {
    titulo: "colo de mãe",
    meta: "29 - 30 Novembro · 18:30",
    texto:
      "Encontro dedicado ao acolhimento, à escuta e às vivências afetivas que fortalecem a comunidade a partir do cuidado.",
  },
  {
    titulo: "CINE VISÃO",
    meta: "6 Novembro · 18:30",
    texto:
      "Sessão de cinema com conversa após a exibição, destacando narrativas negras e o poder do audiovisual como memória.",
  },
  {
    titulo: "dança: samba rock",
    meta: "30 Novembro · 14:00",
    texto:
      "Oficina e baile para aprender passos, experimentar ritmos e celebrar o samba rock como expressão de identidade.",
  },
  {
    titulo: "Teatro: Ana julia",
    meta: "12 Julho · 15:00",
    texto:
      "Apresentação teatral com foco em afeto, denúncia e reflexão sobre experiências do cotidiano e da vivência negra.",
  },
  {
    titulo: "QUINTA COLETIVA",
    meta: "13 Novembro · 19:00",
    texto:
      "Roda de encontro para diálogo, criação conjunta e fortalecimento da participação comunitária por meio da arte.",
  },
  {
    titulo: "aya yoga",
    meta: "16 Outubro · 19:00",
    texto:
      "Prática guiada de yoga com foco em respiração, presença e bem-estar para corpo e mente em um ambiente acolhedor.",
  },
];

const modal = document.querySelector("#event-modal");
const modalTitulo = document.querySelector("#event-modal-title");
const modalMeta = document.querySelector("#event-modal-meta");
const modalTexto = document.querySelector("#event-modal-text");
const modalFechar = document.querySelector(".event-modal__close");
const botoesSaibaMais = document.querySelectorAll(".card-eventos button");

function abrirModalDoEvento(indice) {
  const evento = eventos[indice];

  if (!modal || !evento || !modalTitulo || !modalMeta || !modalTexto) {
    return;
  }

  modalTitulo.textContent = evento.titulo;
  modalMeta.textContent = evento.meta;
  modalTexto.textContent = evento.texto;
  modal.showModal();
}

if (modal && modalFechar) {
  modalFechar.addEventListener("click", () => modal.close());

  modal.addEventListener("click", (event) => {
    const rect = modal.getBoundingClientRect();
    const clicouFora =
      event.clientX < rect.left ||
      event.clientX > rect.right ||
      event.clientY < rect.top ||
      event.clientY > rect.bottom;

    if (clicouFora) {
      modal.close();
    }
  });
}

botoesSaibaMais.forEach((botao, indice) => {
  botao.addEventListener("click", () => abrirModalDoEvento(indice));
});
