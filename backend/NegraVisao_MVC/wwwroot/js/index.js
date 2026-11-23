const modal = document.querySelector('#event-modal');
const modalTitulo = document.querySelector('#event-modal-title');
const modalMeta = document.querySelector('#event-modal-meta');
const modalTexto = document.querySelector('#event-modal-text');
const modalFechar = document.querySelector('.event-modal__close');
const modalCta = document.querySelector('#event-modal-cta');
const botoesSaibaMais = document.querySelectorAll('.btn-saiba-mais');

function abrirModalDoBotao(botao) {
  if (!modal || !modalTitulo || !modalMeta || !modalTexto) {
    return;
  }

  const eventoId = botao.dataset.eventoId;
  const titulo = botao.dataset.titulo || '';
  const meta = botao.dataset.meta || '';
  const texto = botao.dataset.texto || '';
  const abertas = botao.dataset.abertas === 'true';

  modalTitulo.textContent = titulo;
  modalMeta.textContent = meta;
  modalTexto.textContent = texto;

  if (modalCta) {
    if (abertas && eventoId) {
      modalCta.href = `/Inscricao?eventoId=${encodeURIComponent(eventoId)}`;
      modalCta.textContent = 'Inscreva-se';
      modalCta.style.pointerEvents = 'auto';
      modalCta.style.opacity = '1';
    } else {
      modalCta.href = '#';
      modalCta.textContent = 'Inscrições indisponíveis';
      modalCta.style.pointerEvents = 'none';
      modalCta.style.opacity = '0.6';
    }
  }

  modal.showModal();
}

if (modal && modalFechar) {
  modalFechar.addEventListener('click', () => modal.close());

  // Fallback for browsers without declarative <dialog closedby="any"> support
  if (!('closedBy' in HTMLDialogElement.prototype)) {
    modal.addEventListener('click', (event) => {
      if (event.target !== modal) return;

      const rect = modal.getBoundingClientRect();
      const isDialogContent =
        rect.top <= event.clientY &&
        event.clientY <= rect.top + rect.height &&
        rect.left <= event.clientX &&
        event.clientX <= rect.left + rect.width;

      if (isDialogContent) return;
      modal.close();
    });
  }
}

botoesSaibaMais.forEach((botao) => {
  botao.addEventListener('click', () => abrirModalDoBotao(botao));
});
