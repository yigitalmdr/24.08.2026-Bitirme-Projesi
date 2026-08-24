document.addEventListener('DOMContentLoaded', function () {
    document.addEventListener('click', async function (event) {
        const button = event.target.closest('[data-favorite-button]');
        if (!button) return;

        event.preventDefault();
        event.stopPropagation();

        if (button.disabled) return;
        const productId = button.dataset.productId;
        const token = document.querySelector('#antiforgery-token-form input[name="__RequestVerificationToken"]')?.value;
        if (!productId || !token) {
            showFavoriteMessage('İşlem güvenlik doğrulamasından geçemedi. Sayfayı yenileyip tekrar deneyin.', 'error');
            return;
        }

        button.disabled = true;
        button.classList.add('is-loading');

        try {
            const payload = new URLSearchParams({
                productId: productId,
                __RequestVerificationToken: token
            });
            const response = await fetch('/Favorite/ToggleFavorite', {
                method: 'POST',
                credentials: 'same-origin',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                    'RequestVerificationToken': token,
                    'X-Requested-With': 'XMLHttpRequest'
                },
                body: payload.toString()
            });

            if (response.redirected && response.url.includes('/Account/Login')) {
                window.location.href = response.url;
                return;
            }
            if (!response.ok) throw new Error('Favori isteği başarısız oldu.');

            const result = await response.json();
            if (!result.success) throw new Error(result.message || 'Favori işlemi tamamlanamadı.');

            document.querySelectorAll(`[data-favorite-button][data-product-id="${productId}"]`).forEach(function (currentButton) {
                currentButton.classList.toggle('is-favorite', result.isFavorite);
                currentButton.setAttribute('aria-pressed', result.isFavorite ? 'true' : 'false');
                currentButton.title = result.isFavorite ? 'Favorilerden çıkar' : 'Favorilere ekle';
                const icon = currentButton.querySelector('i');
                if (icon) {
                    icon.classList.toggle('fas', result.isFavorite);
                    icon.classList.toggle('far', !result.isFavorite);
                }
            });

            updateFavoriteCount(result.favoriteCount);
            if (!result.isFavorite && button.dataset.removeOnUnfavorite === 'true') {
                const card = document.getElementById(`favorite-card-${productId}`);
                if (card) {
                    card.classList.add('favorite-card-leaving');
                    window.setTimeout(function () {
                        card.remove();
                        updateFavoritePageState(result.favoriteCount);
                    }, 240);
                }
            }
            showFavoriteMessage(result.message, 'success');
        } catch (error) {
            showFavoriteMessage(error.message || 'Favori işlemi sırasında bir sorun oluştu.', 'error');
        } finally {
            button.disabled = false;
            button.classList.remove('is-loading');
        }
    });
});

function updateFavoriteCount(count) {
    const badge = document.getElementById('favorite-count-badge');
    if (!badge) return;
    badge.textContent = count;
    badge.classList.toggle('d-none', !count);
}

function updateFavoritePageState(count) {
    const pageCount = document.getElementById('favorite-page-count');
    if (pageCount) pageCount.textContent = `${count} Ürün`;
    if (count === 0) window.location.reload();
}

function showFavoriteMessage(message, icon) {
    if (window.Swal) {
        Swal.fire({
            toast: true,
            position: 'bottom-end',
            icon: icon,
            title: message,
            showConfirmButton: false,
            timer: 2200,
            timerProgressBar: true
        });
        return;
    }
    window.alert(message);
}
