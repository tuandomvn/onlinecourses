(function () {
    document.addEventListener('DOMContentLoaded', function () {
        const openBtn = document.getElementById('open-video-modal');
        const modal = document.getElementById('video-modal');
        const closeBtn = document.getElementById('close-video-modal');
        const iframe = document.getElementById('video-iframe');
        const youtubeUrl = "https://www.youtube.com/embed/LL35TBsClH4?autoplay=1";

        if (openBtn && modal && closeBtn && iframe) {
            openBtn.addEventListener('click', function () {
                modal.style.display = 'flex';
                iframe.src = youtubeUrl;
            });

            closeBtn.addEventListener('click', function () {
                modal.style.display = 'none';
                iframe.src = "";
            });

            // Close modal when clicking outside content
            modal.addEventListener('click', function (e) {
                if (e.target === modal) {
                    modal.style.display = 'none';
                    iframe.src = "";
                }
            });
        }



        // Optional: Animation for modules on scroll
        function animateModules() {
            const moduleCards = document.querySelectorAll('.module-card');

            moduleCards.forEach((card, index) => {
                // Check if the card is in the viewport
                const cardPosition = card.getBoundingClientRect().top;
                const screenPosition = window.innerHeight / 1.2;

                if (cardPosition < screenPosition) {
                    // Add a small delay for each card
                    setTimeout(() => {
                        card.style.opacity = 1;
                        card.style.transform = 'translateY(0)';
                    }, index * 100);
                }
            });
        }

        // Add initial styles for animation
        const moduleCards = document.querySelectorAll('.module-card');
        moduleCards.forEach(card => {
            card.style.opacity = 0;
            card.style.transform = 'translateY(20px)';
            card.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
        });

        // Run animation on load and scroll
        animateModules();
        window.addEventListener('scroll', animateModules);



        // Elements
        const elements = {
            slider: document.querySelector('.videos-slider'),
            slides: document.querySelectorAll('.video-slide'),
            prevBtn: document.querySelector('.video-prev'),
            nextBtn: document.querySelector('.video-next'),
            modal: document.getElementById('video-modal'),
            iframe: document.getElementById('video-iframe'),
            closeBtn: document.getElementById('close-video-modal'),
            videoCards: document.querySelectorAll('.practical-video-card')
        };

        // Carousel state
        let slideIndex = 0;

        // Carousel functions
        const carousel = {
            // Get number of slides to show based on viewport width
            getSlidesToShow: () => window.innerWidth < 768 ? 1 : window.innerWidth < 992 ? 2 : 3,

            // Move carousel to specific slide index
            goToSlide: (index) => {
                const slidesToShow = carousel.getSlidesToShow();
                const maxIndex = elements.slides.length - slidesToShow;

                // Bound index within valid range
                slideIndex = Math.min(Math.max(0, index), maxIndex);

                // Move slider and update UI
                const slideWidth = elements.slides[0].offsetWidth + 20;
                elements.slider.style.transform = `translateX(-${slideIndex * slideWidth}px)`;

                // Update button states
                if (elements.prevBtn && elements.nextBtn) {
                    elements.prevBtn.style.opacity = slideIndex === 0 ? '0.5' : '1';
                    elements.prevBtn.style.cursor = slideIndex === 0 ? 'default' : 'pointer';
                    elements.nextBtn.style.opacity = slideIndex >= maxIndex ? '0.5' : '1';
                    elements.nextBtn.style.cursor = slideIndex >= maxIndex ? 'default' : 'pointer';
                }
            }
        };

        // Video modal functions
        const videoModal = {
            open: (videoId, startTime = 0) => {
                elements.modal.style.display = 'flex';
                elements.iframe.src = `https://www.youtube.com/embed/${videoId}?autoplay=1&start=${startTime}`;
            },
            close: () => {
                elements.modal.style.display = 'none';
                elements.iframe.src = '';
            }
        };

        // Setup event listeners
        const initEventListeners = () => {
            // Carousel navigation
            elements.prevBtn?.addEventListener('click', () => carousel.goToSlide(slideIndex - 1));
            elements.nextBtn?.addEventListener('click', () => carousel.goToSlide(slideIndex + 1));

            // Handle window resize
            window.addEventListener('resize', () => carousel.goToSlide(slideIndex));

            // Video modal
            elements.videoCards.forEach(card => {
                card.addEventListener('click', () => videoModal.open(card.getAttribute('data-video-id'), card.getAttribute('data-startTime')));
            });

            elements.closeBtn?.addEventListener('click', videoModal.close);
            elements.modal?.addEventListener('click', e => {
                if (e.target === elements.modal) videoModal.close();
            });
        };

        // Initialize
        initEventListeners();
        carousel.goToSlide(0);

    });

})();