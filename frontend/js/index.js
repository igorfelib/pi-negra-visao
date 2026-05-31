const modal = document.querySelector('#modal');
const abrirBtn = document.querySelector('#abrirBtn');
const fecharBtn = document.querySelector('#fecharBtn');
const inscreverseBtn = document.querySelector('#inscreverseBtn');

/* ========== MODAL ========== */
if (abrirBtn && modal) {
  abrirBtn.addEventListener('click', () => modal.showModal());
}

if (fecharBtn && modal) {
  fecharBtn.addEventListener('click', () => modal.close());
}

if (inscreverseBtn) {
  inscreverseBtn.addEventListener('click', () => {
    window.location.href = 'inscricao.html';
  });
}
