const modal = document.querySelector('#modal');
const abrirBtn = document.querySelector('#abrirBtn');
const fecharBtn = document.querySelector('#fecharBtn');
const inscreverseBtn = document.querySelector('#inscreverseBtn');
const menuToggle = document.querySelector('.menu-toggle');
const siteNav = document.querySelector('#site-nav');

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

/* =========== MENU AMBURGUER ===========  */
if (menuToggle && siteNav) {
  const closeMobileMenu = () => {
    siteNav.classList.remove('nav-open');
    menuToggle.setAttribute('aria-expanded', 'false');
  };

  menuToggle.addEventListener('click', () => {
    const isExpanded = menuToggle.getAttribute('aria-expanded') === 'true';
    siteNav.classList.toggle('nav-open', !isExpanded);
    menuToggle.setAttribute('aria-expanded', String(!isExpanded));
  });

  siteNav.querySelectorAll('a').forEach((link) => {
    link.addEventListener('click', closeMobileMenu);
  });

  window.addEventListener('resize', () => {
    if (window.innerWidth > 768) {
      closeMobileMenu();
    }
  });
}
