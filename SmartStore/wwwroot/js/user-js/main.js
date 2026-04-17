(function($) {
	"use strict"

	// Mobile Nav toggle
	$('.menu-toggle > a').on('click', function (e) {
		e.preventDefault();
		$('#responsive-nav').toggleClass('active');
	})

	// Fix cart dropdown from closing
	$('.cart-dropdown').on('click', function (e) {
		e.stopPropagation();
	});

	/////////////////////////////////////////

	// Products Slick
	$('.products-slick').each(function() {
		var $this = $(this),
				$nav = $this.attr('data-nav');

		$this.slick({
			slidesToShow: 4,
			slidesToScroll: 1,
			autoplay: true,
			infinite: true,
			speed: 300,
			dots: false,
			arrows: true,
			appendArrows: $nav ? $nav : false,
			responsive: [{
	        breakpoint: 991,
	        settings: {
	          slidesToShow: 2,
	          slidesToScroll: 1,
	        }
	      },
	      {
	        breakpoint: 480,
	        settings: {
	          slidesToShow: 1,
	          slidesToScroll: 1,
	        }
	      },
	    ]
		});
	});

	// Products Widget Slick
	$('.products-widget-slick').each(function() {
		var $this = $(this),
				$nav = $this.attr('data-nav');

		$this.slick({
			infinite: true,
			autoplay: true,
			speed: 300,
			dots: false,
			arrows: true,
			appendArrows: $nav ? $nav : false,
		});
	});

	/////////////////////////////////////////

	// Product Main img Slick
	$('#product-main-img').slick({
    infinite: true,
    speed: 300,
    dots: false,
    arrows: true,
    fade: true,
    asNavFor: '#product-imgs',
  });

	// Product imgs Slick
  $('#product-imgs').slick({
    slidesToShow: 3,
    slidesToScroll: 1,
    arrows: true,
    centerMode: true,
    focusOnSelect: true,
		centerPadding: 0,
		vertical: true,
    asNavFor: '#product-main-img',
		responsive: [{
        breakpoint: 991,
        settings: {
					vertical: false,
					arrows: false,
					dots: true,
        }
      },
    ]
  });

	// Product img zoom
	var zoomMainProduct = document.getElementById('product-main-img');
	if (zoomMainProduct) {
		$('#product-main-img .product-preview').zoom();
	}

	/////////////////////////////////////////

	// Input number
	$('.input-number').each(function() {
		var $this = $(this),
		$input = $this.find('input[type="number"]'),
		up = $this.find('.qty-up'),
		down = $this.find('.qty-down');

		down.on('click', function () {
			var value = parseInt($input.val()) - 1;
			value = value < 1 ? 1 : value;
			$input.val(value);
			$input.change();
			updatePriceSlider($this , value)
		})

		up.on('click', function () {
			var value = parseInt($input.val()) + 1;
			$input.val(value);
			$input.change();
			updatePriceSlider($this , value)
		})
	});

	var priceInputMax = document.getElementById('price-max'),
			priceInputMin = document.getElementById('price-min');

	priceInputMax.addEventListener('change', function(){
		updatePriceSlider($(this).parent() , this.value)
	});

	priceInputMin.addEventListener('change', function(){
		updatePriceSlider($(this).parent() , this.value)
	});

	function updatePriceSlider(elem , value) {
		if ( elem.hasClass('price-min') ) {
			console.log('min')
			priceSlider.noUiSlider.set([value, null]);
		} else if ( elem.hasClass('price-max')) {
			console.log('max')
			priceSlider.noUiSlider.set([null, value]);
		}
	}

	// Price Slider
	var priceSlider = document.getElementById('price-slider');
	if (priceSlider) {
		noUiSlider.create(priceSlider, {
			start: [1, 999],
			connect: true,
			step: 1,
			range: {
				'min': 1,
				'max': 999
			}
		});

		priceSlider.noUiSlider.on('update', function( values, handle ) {
			var value = values[handle];
			handle ? priceInputMax.value = value : priceInputMin.value = value
		});
	}

	// Collections custom Carousel styling
	if ($('.collections-carousel').length) {
		$('.collections-carousel').slick({
			slidesToShow: 3,
			slidesToScroll: 1,
			autoplay: true,
			autoplaySpeed: 3000,
			infinite: true,
			speed: 600,
			dots: true,
			arrows: false,
			rtl: true,
			responsive: [
				{
					breakpoint: 991,
					settings: {
						slidesToShow: 2,
					}
				},
				{
					breakpoint: 768,
					settings: {
						slidesToShow: 1,
					}
				}
			]
		});
	}

	// Grid/List view toggle
	$('.store-grid li').on('click', function(e) {
		e.preventDefault();
		var $this = $(this);
		$this.addClass('active').siblings().removeClass('active');
		
		var $products = $('#store .row').first().children('div[class*="col-"]');
		var isListView = $this.find('.fa-th-list').length > 0;
		
		if (isListView) {
			$products.removeClass('col-md-4 col-xs-6').addClass('col-md-12 list-view');
			// Hide the visible responsive clearfix
			$('#store .row .clearfix').hide();
		} else {
			$products.removeClass('col-md-12 list-view').addClass('col-md-4 col-xs-6');
			// Show the visible responsive clearfix
			$('#store .row .clearfix').show();
		}
	});

	// Quick View Modal integration
	var quickViewHtml = `
	<div id="quickViewModal" class="modal fade" role="dialog">
		<div class="modal-dialog modal-lg">
			<div class="modal-content" style="border-radius: 12px; overflow: hidden; border: none; box-shadow: 0 15px 35px rgba(0,0,0,0.2);">
				<div class="modal-header" style="border-bottom: 1px solid #eee; padding: 15px 25px;">
					<button type="button" class="close" data-dismiss="modal" style="font-size: 28px; font-weight: 300;">&times;</button>
					<h4 class="modal-title" style="font-weight: 700; color: #2B2D42;">Quick View</h4>
				</div>
				<div class="modal-body" style="padding: 30px;">
					<div class="row">
						<div class="col-md-6 text-center">
							<img id="qv-img" src="" alt="Product" class="img-responsive" style="max-height: 350px; margin: 0 auto; object-fit: contain;">
						</div>
						<div class="col-md-6">
							<div class="product-details" style="padding-top: 20px;">
								<p id="qv-category" class="product-category" style="color: #8D99AE; text-transform: uppercase; font-size: 12px; font-weight: 600; margin-bottom: 5px;">Category</p>
								<h2 id="qv-name" class="product-name" style="font-weight: 700; color: #2B2D42; font-size: 24px; margin-bottom: 15px;">Product Name</h2>
								<div class="product-rating" style="color: #E4E7ED; margin-bottom: 15px;">
									<i class="fa fa-star" style="color: #D10024;"></i>
									<i class="fa fa-star" style="color: #D10024;"></i>
									<i class="fa fa-star" style="color: #D10024;"></i>
									<i class="fa fa-star" style="color: #D10024;"></i>
									<i class="fa fa-star-o"></i>
									<a class="review-link" href="#" style="color: #8D99AE; font-size: 12px; margin-left: 10px;">10 Review(s)</a>
								</div>
								<div>
									<h3 id="qv-price" class="product-price" style="color: #D10024; font-size: 24px; font-weight: 700; display: inline-block;">$980.00</h3>
									<del id="qv-old-price" class="product-old-price" style="color: #8D99AE; font-size: 14px; margin-left: 10px;">$990.00</del>
									<span class="product-available" style="color: #10B981; font-weight: 600; font-size: 12px; margin-left: 15px;">In Stock</span>
								</div>
								<p id="qv-desc" style="color: #6B7280; font-size: 14px; line-height: 1.6; margin: 20px 0;">Great product with modern features and high-quality build. Perfect for your everyday needs.</p>	
								<div class="add-to-cart" style="margin-top: 30px;">
									<div class="qty-label" style="display: inline-block; font-weight: 600; margin-right: 15px;">
										Qty
										<div class="input-number" style="display: inline-block; width: 90px; margin-left: 10px;">
											<input type="number" value="1" style="width: 100%; border: 1px solid #E4E7ED; height: 40px; text-align: center;">
										</div>
									</div>
									<button class="add-to-cart-btn" style="background: #1e1f29; color: #FFF; border: none; height: 40px; padding: 0 25px; border-radius: 40px; font-weight: 700; text-transform: uppercase; transition: 0.3s;"><i class="fa fa-shopping-cart"></i> Add to cart</button>
								</div>
								<ul class="product-btns" style="margin-top: 25px; list-style: none; padding: 0;">
									<li style="display: inline-block; margin-right: 15px;"><a href="#" style="color: #2B2D42; font-weight: 600;"><i class="fa fa-heart-o"></i> Add to wishlist</a></li>
								</ul>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>`;
	
	$('body').append(quickViewHtml);

	$(document).on('click', '.quick-view', function(e) {
		e.preventDefault();
		var $product = $(this).closest('.product');
		if($product.length === 0) return;

		var imgSrc = $product.find('.product-img img').attr('src');
		var category = $product.find('.product-category').text();
		var name = $product.find('.product-name a').length ? $product.find('.product-name a').text() : $product.find('.product-name').text();
		
		var $priceClone = $product.find('.product-price').clone();
		var oldPrice = $priceClone.find('.product-old-price').text();
		$priceClone.find('.product-old-price').remove();
		var price = $priceClone.text().trim();

		$('#qv-img').attr('src', imgSrc);
		$('#qv-category').text(category);
		$('#qv-name').text(name);
		$('#qv-price').text(price);
		
		if(oldPrice && oldPrice.trim() !== '') {
			$('#qv-old-price').text(oldPrice).show();
		} else {
			$('#qv-old-price').hide();
		}

		$('#quickViewModal').modal('show');
	});

})(jQuery);
